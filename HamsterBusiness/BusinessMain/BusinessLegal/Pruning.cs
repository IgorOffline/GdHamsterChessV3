using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessGame;
using HamsterBusiness.BusinessMain.BusinessUtil;

namespace HamsterBusiness.BusinessMain.BusinessLegal;

public static class Pruning
{
    public static void Prune(GameMaster gameMaster, PieceColor pieceColor) {

        var oppositePieceColor = pieceColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var sq = gameMaster.PBoard.PBoard[row][col];
                // TODO Pieces
            }
        }
    }
    
    private static void KingStillInCheck(GameMaster gameMaster, PieceColor pieceColor) {
        switch (pieceColor)
        {
            case PieceColor.White:
                gameMaster.WhiteKingInCheck = true;
                break;
            case PieceColor.Black:
                gameMaster.BlackKingInCheck = true;
                break;
            default: throw new InvalidOperationException(Messages.UnknownPieceColor);
        }
    }
}