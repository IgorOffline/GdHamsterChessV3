using HamsterBusiness.BusinessMain.BusinessBoard;

namespace HamsterBusiness.BusinessMain.BusinessPiece.BusinessMovement;

public record MovementAttackOpponentCheck(List<Square> MovementSquares, List<Square> AttackSquares, bool OpponentsKingInCheck);