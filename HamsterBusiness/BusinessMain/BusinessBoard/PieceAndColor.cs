using HamsterBusiness.BusinessMain.BusinessUtil;

namespace HamsterBusiness.BusinessMain.BusinessBoard;

public static class PieceAndColor
{
    public static string GetPieceShortString(Piece piece)
    {
        return piece switch
        {
            Piece.King => "K",
            Piece.Rook => "R",
            Piece.Bishop => "B",
            Piece.None => "-",
            _ => throw new ArgumentException(Messages.UnknownPiece)
        };
    }

    public static string GetPieceColorShortString(PieceColor pieceColor)
    {
        return pieceColor switch
        {
            PieceColor.White => "W",
            PieceColor.Black => "B",
            PieceColor.None => "-",
            _ => throw new ArgumentException(Messages.UnknownPieceColor)
        };
    }
}