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
    
    public void MoveAndCalculate(int fromIndex, int toIndex)
    {
        FromSquare = null;
        ToSquare = null;
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var sq = PBoard.PBoard[row][col];
                if (sq.GetIndex() == fromIndex && sq.Piece != Piece.None)
                {
                    FromSquare = sq;
                }
                if (sq.GetIndex() == toIndex)
                {
                    ToSquare = sq;
                }
            }
        }
        MoveAndCalculateInner();
    }
    
    private void MoveAndCalculateInner()
    {
        if (PLegalMoves.PLegalMoves.TryGetValue(FromSquare!, out List<Square>? value))
        {
            foreach (var pieceLegalMove in value)
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