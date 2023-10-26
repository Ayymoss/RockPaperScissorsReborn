using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Domain.Interfaces;

public interface IGameLogicService
{
    public Player? IsUserWinner((Player Player, GameMove Move) playerOne, (Player Player, GameMove Move) playerTwo);
    bool IsUserWinner(GameMove userMove, GameMove computerMove);
    GameMove GenerateComputerMove();
    int CalculatePayout(int streak);
}
