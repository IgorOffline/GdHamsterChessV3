namespace HamsterBusiness.BusinessMain.BusinessBoard;

public class Board
{
    public Square[][] PBoard { get; }
    
    public Board() {
        PBoard = new Square[8][];
        CreateBoard();
    }
    
    public Board(Square[][] board) {
        PBoard = board;
    }

    private void CreateBoard()
    {
        var filledSquares = new List<Square>
        {
            new(Letter.E, Number2.N6, Piece.King, PieceColor.White),
            new(Letter.A, Number2.N6, Piece.Rook, PieceColor.White),
            new(Letter.E, Number2.N8, Piece.King, PieceColor.Black)
        };

        for (var row = 0; row < 8; row++)
        {
            PBoard[row] = new Square[8];
            for (var col = 0; col < 8; col++)
            {
                var letter = LetterNumber.GetLetterEnum(row);
                var number = LetterNumber.GetNumberEnumReverse(col);
                var letterNumberInFilled = false;
                foreach (var filled in filledSquares)
                {
                    if (filled.Letter == letter && filled.Number == number)
                    {
                        PBoard[row][col] = filled;
                        letterNumberInFilled = true;
                        break;
                    }
                }
                if (!letterNumberInFilled)
                {
                    var sqaure = new Square(letter, number, Piece.None, PieceColor.None);
                    PBoard[row][col] = sqaure;
                }
            }
        }
    }
    
    public Square? FindNextNumberSquare(Letter letter, Number2 number) {
        var nextNumberIndex = (ushort)number + 1;
        if (LetterNumber.IsLetterNumberIllegal(nextNumberIndex)) {
            return null;
        }

        var col = 7 - nextNumberIndex;

        return PBoard[(ushort)letter][col];
    }
    
    public Square? FindPreviousNumberSquare(Letter letter, Number2 number) {
        var previousNumberIndex = (ushort)number - 1;
        if (LetterNumber.IsLetterNumberIllegal(previousNumberIndex)) {
            return null;
        }

        var col = 7 - previousNumberIndex;

        return PBoard[(ushort)letter][col];
    }
    
    public Square? FindNextLetterSquare(Letter letter, Number2 number) {
        var nextLetterIndex = (ushort)letter + 1;
        if (LetterNumber.IsLetterNumberIllegal(nextLetterIndex)) {
            return null;
        }

        var col = 7 - (ushort)number;

        return PBoard[nextLetterIndex][col];
    }
    
    public Square? FindPreviousLetterSquare(Letter letter, Number2 number) {
        var previousLetterIndex = (ushort)letter - 1;
        if (LetterNumber.IsLetterNumberIllegal(previousLetterIndex)) {
            return null;
        }

        var col = 7 - (ushort)number;

        return PBoard[previousLetterIndex][col];
    }
    
    public Square? FindPreviousLetterNextNumberSquare(Letter letter, Number2 number) {
        var previousLetterIndex = (ushort)letter - 1;
        var nextNumberIndex = (ushort)number + 1;

        if (LetterNumber.IsLetterNumberIllegal(previousLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(nextNumberIndex)) {
            return null;
        }

        var col = 7 - nextNumberIndex;

        return PBoard[previousLetterIndex][col];
    }
    
    public Square? FindNextLetterNextNumberSquare(Letter letter, Number2 number) {
        var nextLetterIndex = (ushort)letter + 1;
        var nextNumberIndex = (ushort)number + 1;

        if (LetterNumber.IsLetterNumberIllegal(nextLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(nextNumberIndex)) {
            return null;
        }

        var col = 7 - nextNumberIndex;

        return PBoard[nextLetterIndex][col];
    }
    
    public Square? FindPreviousLetterPreviousNumberSquare(Letter letter, Number2 number) {
        var previousLetterIndex = (ushort)letter - 1;
        var previousNumberIndex = (ushort)number - 1;

        if (LetterNumber.IsLetterNumberIllegal(previousLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(previousNumberIndex)) {
            return null;
        }

        var col = 7 - previousNumberIndex;

        return PBoard[previousLetterIndex][col];
    }
    
    public Square? FindNextLetterPreviousNumberSquare(Letter letter, Number2 number) {
        var nextLetterIndex = (ushort)letter + 1;
        var previousNumberIndex = (ushort)number - 1;

        if (LetterNumber.IsLetterNumberIllegal(nextLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(previousNumberIndex)) {
            return null;
        }

        var col = 7 - previousNumberIndex;

        return PBoard[nextLetterIndex][col];
    }
    
    public Square? FindPpLetterNextNumberSquare(Letter letter, Number2 number) {
        var ppLetterIndex = (ushort)letter - 2;
        var nextNumberIndex = (ushort)number + 1;

        if (LetterNumber.IsLetterNumberIllegal(ppLetterIndex) || LetterNumber.IsLetterNumberIllegal(nextNumberIndex)) {
            return null;
        }

        var col = 7 - nextNumberIndex;

        return PBoard[ppLetterIndex][col];
    }
    
    public Square? FindNnNumberPreviousLetterSquare(Letter letter, Number2 number) {
        var previousLetterIndex = (ushort)letter - 1;
        var nnNumberIndex = (ushort)number + 2;

        if (LetterNumber.IsLetterNumberIllegal(previousLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(nnNumberIndex)) {
            return null;
        }

        var col = 7 - nnNumberIndex;

        return PBoard[previousLetterIndex][col];
    }
    
    public Square? FindNnNumberNextLetterSquare(Letter letter, Number2 number) {
        var nextLetterIndex = (ushort)letter + 1;
        var nnNumberIndex = (ushort)number + 2;

        if (LetterNumber.IsLetterNumberIllegal(nextLetterIndex) || LetterNumber.IsLetterNumberIllegal(nnNumberIndex)) {
            return null;
        }

        var col = 7 - nnNumberIndex;

        return PBoard[nextLetterIndex][col];
    }
    
    public Square? FindNnLetterNextNumberSquare(Letter letter, Number2 number) {
        var nnLetterIndex = (ushort)letter + 2;
        var nextNumberIndex = (ushort)number + 1;

        if (LetterNumber.IsLetterNumberIllegal(nnLetterIndex) || LetterNumber.IsLetterNumberIllegal(nextNumberIndex)) {
            return null;
        }

        var col = 7 - nextNumberIndex;

        return PBoard[nnLetterIndex][col];
    }
    
    public Square? FindPpLetterPreviousNumberSquare(Letter letter, Number2 number) {
        var ppLetterIndex = (ushort)letter - 2;
        var previousNumberIndex = (ushort)number - 1;

        if (LetterNumber.IsLetterNumberIllegal(ppLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(previousNumberIndex)) {
            return null;
        }

        var col = 7 - previousNumberIndex;

        return PBoard[ppLetterIndex][col];
    }
    
    public Square? FindPpNumberPreviousLetterSquare(Letter letter, Number2 number) {
        var previousLetterIndex = (ushort)letter - 1;
        var ppNumberIndex = (ushort)number - 2;

        if (LetterNumber.IsLetterNumberIllegal(previousLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(ppNumberIndex)) {
            return null;
        }

        var col = 7 - ppNumberIndex;

        return PBoard[previousLetterIndex][col];
    }
    
    public Square? FindPpNumberNextLetterSquare(Letter letter, Number2 number) {
        var nextLetterIndex = (ushort)letter + 1;
        var ppNumberIndex = (ushort)number - 2;

        if (LetterNumber.IsLetterNumberIllegal(nextLetterIndex) || LetterNumber.IsLetterNumberIllegal(ppNumberIndex)) {
            return null;
        }

        var col = 7 - ppNumberIndex;

        return PBoard[nextLetterIndex][col];
    }
    
    public Square? FindNnLetterPreviousNumberSquare(Letter letter, Number2 number) {
        var nnLetterIndex = (ushort)letter + 2;
        var previousNumberIndex = (ushort)number - 1;

        if (LetterNumber.IsLetterNumberIllegal(nnLetterIndex)
            || LetterNumber.IsLetterNumberIllegal(previousNumberIndex)) {
            return null;
        }

        var col = 7 - previousNumberIndex;

        return PBoard[nnLetterIndex][col];
    }
    
    public Board DeepCopy()
    {
        var newBoard = new Square[8][];
        for (var row = 0; row < 8; row++)
        {
            newBoard[row] = new Square[8];
            for (var col = 0; col < 8; col++)
            {
                newBoard[row][col] = new Square(PBoard[row][col].Letter, PBoard[row][col].Number, PBoard[row][col].Piece, PBoard[row][col].PieceColor);
            }
        }

        return new Board(newBoard);
    }
}