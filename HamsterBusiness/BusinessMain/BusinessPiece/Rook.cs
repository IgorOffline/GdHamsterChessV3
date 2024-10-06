using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

namespace HamsterBusiness.BusinessMain.BusinessPiece;

public static class Rook
{
    public static MovementAttackOpponentCheck RookMoves(Square rookSquare, Board board) {

        var list = new List<Square>();

        var movement1 = FindSquare.DoFindSquare(Piece.Rook, PieceMovement.NextNumber, rookSquare, board);
        var movement2 = FindSquare.DoFindSquare(Piece.Rook, PieceMovement.PreviousNumber, rookSquare, board);
        var movement3 = FindSquare.DoFindSquare(Piece.Rook, PieceMovement.NextLetter, rookSquare, board);
        var movement4 = FindSquare.DoFindSquare(Piece.Rook, PieceMovement.PreviousLetter, rookSquare, board);

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