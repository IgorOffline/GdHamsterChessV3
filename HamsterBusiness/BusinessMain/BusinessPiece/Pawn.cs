using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

namespace HamsterBusiness.BusinessMain.BusinessPiece;

public static class Pawn
{
    public static MovementAttackOpponentCheck PawnMoves(Square pawnSquare, Board board)
    {
        var movementSquares = new List<Square>();
        var attackSquares = new List<Square>();
        
        var movement1 = FindSquare.DoFindSquare(Piece.Pawn, PieceMovement.PawnMoveOneSquare, pawnSquare, board);
        MovementContact? movement2 = null;

        var white2 = pawnSquare is { PieceColor: PieceColor.White, Number: Number2.N2 };
        var black7 = pawnSquare is { PieceColor: PieceColor.Black, Number: Number2.N7 };
        var white2OrBlack7 = white2 || black7;

        if (white2OrBlack7 && movement1.Contact == Contact.None)
        {
            movement2 = FindSquare.DoFindSquare(Piece.Pawn, PieceMovement.PawnMoveTwoSquares, pawnSquare, board);
        }
        
        var attack1 = FindSquare.DoFindSquare(Piece.Pawn, PieceMovement.PawnAttackPreviousLetter, pawnSquare, board);
        var attack2 = FindSquare.DoFindSquare(Piece.Pawn, PieceMovement.PawnAttackNextLetter, pawnSquare, board);
        
        var movements = new List<MovementContact> { movement1 };
        if (movement2 != null)
        {
            movements.Add(movement2);
        }

        foreach (var movement in movements.Where(movement => movement.Contact != Contact.None))
        {
            movementSquares.AddRange(movement.Squares);
        }

        var attacks = new List<MovementContact?>
        {
            attack1, attack2
        };
        foreach (var attack in attacks.OfType<MovementContact>())
        {
            attackSquares.AddRange(attack.Squares);
        }
        foreach (var attack in attacks.OfType<MovementContact>().Where(attack => attack.Contact == Contact.OpponentNonKing))
        {
            movementSquares.AddRange(attack.Squares);
        }

        var opponentsKingInCheck = attacks.Any(movementContact => movementContact is { Contact: Contact.OpponentKing });

        return new MovementAttackOpponentCheck(movementSquares, attackSquares, opponentsKingInCheck);
    }
}