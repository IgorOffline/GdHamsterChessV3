using HamsterBusiness.BusinessMain.BusinessBoard;

namespace HamsterBusiness.BusinessMain.BusinessPiece;

public static class King
{
    public static List<Square> KingMoves(Square kingSquare, Board board)
    {
        var potentialMoves = KingMovesInner(kingSquare);
        var oppositeColor = kingSquare.PieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
        var oppositeKingSquare = OppositeKingSquare(board, oppositeColor);
        var illegalMoves = KingMovesInner(oppositeKingSquare);
        potentialMoves.RemoveAll(e => illegalMoves.Contains(e));

        return potentialMoves;
    }
    
    private static Square OppositeKingSquare(Board board, PieceColor oppositeColor)
    {
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var square = board.PBoard[row][col];
                if (square.Piece == Piece.King && square.PieceColor == oppositeColor)
                {
                    return square;
                }
            }
        }

        throw new InvalidOperationException("Opposite king not found");
    }
    
    private static List<Square> KingMovesInner(Square kingSquare) {

        var moves = new List<Square>();

        var letterIndexMinus = (ushort)kingSquare.Letter - 1;
        var letterIndexPlus = (ushort)kingSquare.Letter + 1;
        var numberIndexMinus = (ushort)kingSquare.Number - 1;
        var numberIndexPlus = (ushort)kingSquare.Number + 1;

        var letter = kingSquare.Letter;
        var letterMinus = GetKingLetterNumber(letterIndexMinus, true);
        var letterPlus = GetKingLetterNumber(letterIndexPlus, true);
        var number = kingSquare.Number;
        var numberMinus = GetKingLetterNumber(numberIndexMinus, false);
        var numberPlus = GetKingLetterNumber(numberIndexPlus, false);

        if (letterMinus.Legal)
        {
            moves.Add(new Square(letterMinus.Letter!.Value, number, Piece.None, PieceColor.None));
            if (numberPlus.Legal) {
                moves.Add(new Square(letterMinus.Letter.Value, numberPlus.Number!.Value, Piece.None, PieceColor.None));
            }
            if (numberMinus.Legal) {
                moves.Add(new Square(letterMinus.Letter.Value, numberMinus.Number!.Value, Piece.None, PieceColor.None));
            }
        }
        if (letterPlus.Legal) {
            moves.Add(new Square(letterPlus.Letter!.Value, number, Piece.None, PieceColor.None));
            if (numberPlus.Legal) {
                moves.Add(new Square(letterPlus.Letter.Value, numberPlus.Number!.Value, Piece.None, PieceColor.None));
            }
            if (numberMinus.Legal) {
                moves.Add(new Square(letterPlus.Letter.Value, numberMinus.Number!.Value, Piece.None, PieceColor.None));
            }
        }
        if (numberPlus.Legal) {
            moves.Add(new Square(letter, numberPlus.Number!.Value, Piece.None, PieceColor.None));
        }
        if (numberMinus.Legal) {
            moves.Add(new Square(letter, numberMinus.Number!.Value, Piece.None, PieceColor.None));
        }

        return moves;
    }
    
    private record KingLetterNumber(bool Legal, Letter? Letter, Number2? Number);

    private static KingLetterNumber GetKingLetterNumber(int index, bool isLetter) {
        if (LetterNumber.IsLetterNumberIllegal(index)) {
            return new KingLetterNumber(false, null, null);
        }

        Letter? letter = isLetter ? LetterNumber.GetLetterEnum(index) : null;
        Number2? number = isLetter ? null : LetterNumber.GetNumberEnum(index);

        return new KingLetterNumber(true, letter, number);
    }
}