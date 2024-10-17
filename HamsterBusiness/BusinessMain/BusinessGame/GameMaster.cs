using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessLegal;

namespace HamsterBusiness.BusinessMain.BusinessGame;

public class GameMaster
{
    public Board PBoard { get; }
    public LegalMoves PLegalMoves { get; }

    public Square? FromSquare { get; set; }
    public Square? ToSquare { get; set; }
    public bool WhiteToMove { get; set; } = true;
    public bool WhiteKingInCheck { get; set; } = false;
    public bool BlackKingInCheck { get; set; } = false;
    public bool WhiteKingCheckmated { get; set; } = false;
    public bool BlackKingCheckmated { get; set; } = false;

    public GameMaster()
    {
        PBoard = new Board();
        PLegalMoves = new LegalMoves();
        PLegalMoves.Calculate(this, false);
    }

    public GameMaster(Board board, LegalMoves legalMoves)
    {
        PBoard = board;
        PLegalMoves = legalMoves;
    }
    
    public void MoveAndCalculate(int fromRow, int fromColumn, int toRow, int toColumn)
    {
        if (fromRow == -1 && fromColumn == -1 && toRow == -1 && toColumn == -1)
        {
            return;
        }
        
        FromSquare = PBoard.PBoard[fromRow][fromColumn];
        if (FromSquare.Piece == Piece.None)
        {
            FromSquare = null;
        }
        ToSquare = PBoard.PBoard[toRow][toColumn];
        
        MoveAndCalculateInner();
    }
    
    private void MoveAndCalculateInner()
    {
        if (PLegalMoves.PLegalMoves.TryGetValue(FromSquare!, out List<Square>? pieceLegalMoves))
        {
            foreach (var pieceLegalMove in pieceLegalMoves)
            {
                if (ToSquareEquals(pieceLegalMove)) {
                    PLegalMoves.Move(this);
                    PLegalMoves.Calculate(this, true);

                    return;
                }
            }
        }
    }
    
    private bool ToSquareEquals(Square square)
    {
        return Square.IsLetterNumberEqual(ToSquare!.Letter, square.Letter, ToSquare.Number, square.Number);
    }
}