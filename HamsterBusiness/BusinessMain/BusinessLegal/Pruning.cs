using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessGame;
using HamsterBusiness.BusinessMain.BusinessPiece;
using HamsterBusiness.BusinessMain.BusinessUtil;

namespace HamsterBusiness.BusinessMain.BusinessLegal;

public static class Pruning
{
    public static void Prune(GameMaster gameMaster, PieceColor pieceColor) {

        var oppositePieceColor = pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var sq = gameMaster.PBoard.PBoard[row][col];
                if (sq.PieceColor == oppositePieceColor && sq.Piece == Piece.Rook)
                {
                    var moves = Rook.RookMoves(sq, gameMaster.PBoard);
                    if (moves.OpponentsKingInCheck)
                    {
                        KingStillInCheck(gameMaster, pieceColor);
                    }
                }
                else if (sq.PieceColor == oppositePieceColor && sq.Piece == Piece.Bishop)
                {
                    var moves = Bishop.BishopMoves(sq, gameMaster.PBoard);
                    if (moves.OpponentsKingInCheck)
                    {
                        KingStillInCheck(gameMaster, pieceColor);
                    }
                }
                else if (sq.PieceColor == oppositePieceColor && sq.Piece == Piece.Knight)
                {
                    var moves = Knight.KnightMoves(sq, gameMaster.PBoard);
                    if (moves.OpponentsKingInCheck)
                    {
                        KingStillInCheck(gameMaster, pieceColor);
                    }
                }
                else if (sq.PieceColor == oppositePieceColor && sq.Piece == Piece.Pawn)
                {
                    var moves = Pawn.PawnMoves(sq, gameMaster.PBoard);
                    if (moves.OpponentsKingInCheck)
                    {
                        KingStillInCheck(gameMaster, pieceColor);
                    }
                }
            }
        }
    }
    
    private static void KingStillInCheck(GameMaster gameMaster, PieceColor pieceColor) {
        switch (pieceColor)
        {
            case PieceColor.White:
                gameMaster.WhiteKingInCheck = true;
                return;
            case PieceColor.Black:
                gameMaster.BlackKingInCheck = true;
                return;
            case PieceColor.None:
            default: throw new InvalidOperationException(Messages.UnknownPieceColor);
        }
    }
}