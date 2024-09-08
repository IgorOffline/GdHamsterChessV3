using HamsterBusiness.BusinessMain.BusinessBoard;

namespace HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

public static class FindSquare
{
    public static MovementContact DoFindSquare(Piece piece, PieceMovement pieceMovement, Square pieceSquare, Board board)
    {
        var moves = new List<Square>();

        var pieceColor = pieceSquare.PieceColor == PieceColor.White ? PieceColor.White : PieceColor.Black;
        var oppositePieceColor = pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
        
        var doWhilePieces = new List<Piece>
        { 
            Piece.Rook, 
            Piece.Bishop   
        };

        Square? square = new Square(pieceSquare.Letter, pieceSquare.Number, Piece.None, PieceColor.None);
        var contact = Contact.None;

        do
        {
            if (piece == Piece.Rook)
            {
                square = pieceMovement switch
                {
                    PieceMovement.NextNumber => board.FindNextNumberSquare(square.Letter, square.Number),
                    PieceMovement.PreviousNumber => board.FindPreviousNumberSquare(square.Letter, square.Number),
                    PieceMovement.NextLetter => board.FindNextLetterSquare(square.Letter, square.Number),
                    PieceMovement.PreviousLetter => board.FindPreviousLetterSquare(square.Letter, square.Number),
                    _ => throw new InvalidOperationException("Illegal ROOK movement")
                };
            }
            // TODO other pieces

            if (square != null)
            {
                if (square.Piece == Piece.None)
                {
                    moves.Add(square);
                }
                else if (square.PieceColor == pieceColor)
                {
                    contact = Contact.Friendly;
                }
                else if (square.PieceColor == oppositePieceColor)
                {
                    if (square.Piece == Piece.King)
                    {
                        contact = Contact.OpponentKing;
                    }
                    else
                    {
                        moves.Add(square);
                        contact = Contact.OpponentNonKing;
                    }
                }
            }

        } while (square != null && contact == Contact.None && doWhilePieces.Contains(piece));

        return new MovementContact(moves, contact);
    }
}