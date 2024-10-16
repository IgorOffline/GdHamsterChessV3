using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

namespace HamsterBusiness.BusinessMain.BusinessPiece;

public static class Bishop
{
    public static MovementAttackOpponentCheck BishopMoves(Square bishopSquare, Board board)
    {
        var list = new List<Square>();
        
        var movement1 = FindSquare.DoFindSquare(Piece.Bishop, PieceMovement.PreviousLetterNextNumber, bishopSquare, board);
        var movement2 = FindSquare.DoFindSquare(Piece.Bishop, PieceMovement.NextLetterNextNumber, bishopSquare, board);
        var movement3 = FindSquare.DoFindSquare(Piece.Bishop, PieceMovement.PreviousLetterPreviousNumber, bishopSquare, board);
        var movement4 = FindSquare.DoFindSquare(Piece.Bishop, PieceMovement.NextLetterPreviousNumber, bishopSquare, board);
        
        var movements = new List<MovementContact>
        {
            movement1,
            movement2,
            movement3,
            movement4
        };
        foreach (var movement in movements)
        {
            list.AddRange(movement.Squares);
        }

        var opponentsKingInCheck = movements.Any(movementContact => movementContact.Contact is Contact.OpponentKing);

        return new MovementAttackOpponentCheck(list, [], opponentsKingInCheck);
    }
}