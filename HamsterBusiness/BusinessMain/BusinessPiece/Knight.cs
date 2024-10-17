using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

namespace HamsterBusiness.BusinessMain.BusinessPiece;

public static class Knight
{
    public static MovementAttackOpponentCheck KnightMoves(Square knightSquare, Board board)
    {
        var list = new List<Square>();
        
        var movement1 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.PpLetterNextNumber, knightSquare, board);
        var movement2 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.NnNumberPreviousLetter, knightSquare, board);
        var movement3 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.NnNumberNextLetter, knightSquare, board);
        var movement4 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.NnLetterNextNumber, knightSquare, board);
        var movement5 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.PpLetterPreviousNumber, knightSquare, board);
        var movement6 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.PpNumberPreviousLetter, knightSquare, board);
        var movement7 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.PpNumberNextLetter, knightSquare, board);
        var movement8 = FindSquare.DoFindSquare(Piece.Knight, PieceMovement.NnLetterPreviousNumber, knightSquare, board);
        
        var movements = new List<MovementContact>
        {
            movement1,
            movement2,
            movement3,
            movement4,
            movement5,
            movement6,
            movement7,
            movement8
        };
        foreach (var movement in movements)
        {
            list.AddRange(movement.Squares);
        }

        var opponentsKingInCheck = movements.Any(movementContact => movementContact.Contact is Contact.OpponentKing);

        return new MovementAttackOpponentCheck(list, [], opponentsKingInCheck);
    }
}