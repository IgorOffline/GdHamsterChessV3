using System.Diagnostics;
using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessGame;
using HamsterBusiness.BusinessMain.BusinessPiece;

namespace HamsterBusiness.BusinessMain.BusinessLegal;

public class LegalMoves
{
    public Dictionary<Square, List<Square>> PLegalMoves { get; set; }
    
    public void Move(GameMaster gameMaster)
    {
        var fromSquare = gameMaster.FromSquare!;
        var toSquare = gameMaster.ToSquare!;
        toSquare.Piece = fromSquare.Piece;
        toSquare.PieceColor = fromSquare.PieceColor;
        fromSquare.Piece = Piece.None;
        fromSquare.PieceColor = PieceColor.None;
    }
    
    public void Calculate(GameMaster gameMaster, bool switchWhiteToMove)
    {
        if (switchWhiteToMove)
        {
            gameMaster.WhiteToMove = !gameMaster.WhiteToMove;
        }

        var phase1LegalMoves = new Dictionary<Square, List<Square>>();
        var phase2LegalMoves = new Dictionary<Square, List<Square>>();

        var pieceColor = gameMaster.WhiteToMove ? PieceColor.White : PieceColor.Black;
        var oppositePieceColor = pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        gameMaster.WhiteKingInCheck = false;
        gameMaster.BlackKingInCheck = false;

        var king = FindKing(gameMaster, pieceColor);

        var kingLegalMoves = King.KingMoves(king, gameMaster.PBoard);

        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var boardSquare = gameMaster.PBoard.PBoard[row][col];
                if (boardSquare.Piece == Piece.Rook && boardSquare.PieceColor == pieceColor)
                {
                    var rookMoves = Rook.RookMoves(boardSquare, gameMaster.PBoard);
                    phase1LegalMoves[boardSquare] = rookMoves.MovementSquares;
                    kingLegalMoves.RemoveAll(square =>
                        square.Letter == boardSquare.Letter && square.Number == boardSquare.Number);
                }
                else if (boardSquare.Piece == Piece.Rook && boardSquare.PieceColor == oppositePieceColor)
                {
                    var oppositeRookMoves = Rook.RookMoves(boardSquare, gameMaster.PBoard);
                    kingLegalMoves.RemoveAll(e => oppositeRookMoves.MovementSquares.Contains(e));
                    if (oppositeRookMoves.OpponentsKingInCheck)
                    {
                        gameMaster.WhiteKingInCheck = pieceColor == PieceColor.White;
                        gameMaster.BlackKingInCheck = pieceColor == PieceColor.Black;
                    }
                }
                // TODO Pieces
            }
        }

        phase1LegalMoves[king] = kingLegalMoves;

        foreach (var piece in phase1LegalMoves.Keys)
        {
            var pieceLegalMoves = phase1LegalMoves[piece];
            var prunedMoves = PruneMoves(gameMaster, pieceLegalMoves, piece);
            phase2LegalMoves[piece] = prunedMoves; 
        }
        
        PLegalMoves = phase2LegalMoves;

        if (gameMaster.WhiteKingInCheck || gameMaster.BlackKingInCheck) {
            CheckmateCheck(gameMaster);
        }
    }
    
    private Square FindKing(GameMaster gameMaster, PieceColor pieceColor)
    {
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var square = gameMaster.PBoard.PBoard[row][col];
                if (square.Piece == Piece.King && square.PieceColor == pieceColor) {
                    return square;
                }
            }
        }

        throw new InvalidOperationException("King not found");
    }
    
    private List<Square> PruneMoves(GameMaster gameMaster, List<Square> pieceLegalMoves, Square piece) {

        var prunedMoves = new List<Square>();

        foreach (var legalMove in pieceLegalMoves)
        {
            var newBoard = gameMaster.PBoard.DeepCopy();
            var newLegalMoves = new LegalMoves();
            var newGameMaster = new GameMaster(newBoard, newLegalMoves);
            var pieceNewBoard = FindPieceOnNewBoard(piece, newBoard);
            Debug.Assert(pieceNewBoard != null, "pieceNewBoard is null!");
            newGameMaster.FromSquare = pieceNewBoard;
            var toSquareNewBoard = FindPieceOnNewBoard(legalMove, newBoard);
            Debug.Assert(toSquareNewBoard != null, "toSquareNewBoard is null!");
            newGameMaster.ToSquare = toSquareNewBoard;
            newLegalMoves.Move(newGameMaster);
            Pruning.Prune(newGameMaster, piece.PieceColor);

            var pruneWhite = piece.PieceColor == PieceColor.White && !newGameMaster.WhiteKingInCheck;
            var pruneBlack = piece.PieceColor == PieceColor.Black && !newGameMaster.BlackKingInCheck;

            if (pruneWhite || pruneBlack) {
                prunedMoves.Add(toSquareNewBoard);
            }
        }

        return prunedMoves;
    }
    
    private Square? FindPieceOnNewBoard(Square piece, Board newBoard)
    {
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var square = newBoard.PBoard[row][col];
                if (square.Letter == piece.Letter && square.Number == piece.Number)
                {
                    return square;
                }
            }
        }

        return null;
    }
    
    private void CheckmateCheck(GameMaster gameMaster) {

        var legalMovesCounter = PLegalMoves.Values.Sum(piecesLegalMoves => piecesLegalMoves.Count);

        if (legalMovesCounter != 0)
        {
            return;
        }
        
        if (gameMaster.WhiteKingInCheck)
        {
            gameMaster.WhiteKingCheckmated = true;
        }
        else
        {
            gameMaster.BlackKingCheckmated = true;
        }
    }
}