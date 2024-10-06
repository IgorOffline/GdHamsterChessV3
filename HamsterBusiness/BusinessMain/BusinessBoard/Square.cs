namespace HamsterBusiness.BusinessMain.BusinessBoard;

public class Square(Letter letter, Number2 number, Piece piece, PieceColor pieceColor)
{
    public Letter Letter { get; } = letter;
    public Number2 Number { get; } = number;
    public Piece Piece { get; set; } = piece;
    public PieceColor PieceColor { get; set; } = pieceColor;

    public int GetIndex()
    {
        return (ushort)Letter + 8 * LetterNumber.GetNumberIndexReverse(Number);
    }
    
    public static bool IsLetterNumberEqual(Letter letter1, Letter letter2, Number2 number1, Number2 number2) {
        return letter1 == letter2 && number1 == number2;
    }
    
    public Square Copy() {
        return new Square(letter, number, piece, pieceColor);
    }
    
    protected bool Equals(Square other)
    {
        return Letter == other.Letter && Number == other.Number && Piece == other.Piece && PieceColor == other.PieceColor;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Square)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Letter, (int)Number, (int)Piece, (int)PieceColor);
    }

    public string ToShortString()
    {
        var pieceAndColor = $"{PieceAndColor.GetPieceColorShortString(PieceColor)}{PieceAndColor.GetPieceShortString(Piece)}";
        var letterNumber = $"{LetterNumber.GetLetterToShortString(Letter)}{LetterNumber.GetNumberToShortString(Number)}";
        return $"{pieceAndColor}-{letterNumber}";
    }
    
    public override string ToString()
    {
        return $"Square(Letter={Letter}, Number={Number}, Piece={Piece}, PieceColor={PieceColor})";
    }
}