using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.Services;

// https://chat.openai.com/c/8b842bd8-d4f8-4e77-aacb-a568dd06baa1

public class GameLogicService : IGameLogicService
{
    public Player? IsUserWinner((Player Player, GameMove Move) playerOne, (Player Player, GameMove Move) playerTwo)
    {
        if (playerOne.Move == playerTwo.Move) return null;

        return playerOne.Move switch
        {
            GameMove.Rock when playerTwo.Move is GameMove.Scissors => playerOne.Player,
            GameMove.Paper when playerTwo.Move is GameMove.Rock => playerOne.Player,
            GameMove.Scissors when playerTwo.Move is GameMove.Paper => playerOne.Player,
            _ => playerTwo.Player
        };
    }

    public bool IsUserWinner(GameMove userMove, GameMove computerMove)
    {
        return userMove switch
        {
            GameMove.Rock => computerMove is GameMove.Scissors,
            GameMove.Paper => computerMove is GameMove.Rock,
            GameMove.Scissors => computerMove is GameMove.Paper,
            _ => false
        };
    }

    public GameMove GenerateComputerMove()
    {
        return (GameMove)Random.Shared.Next(0, 3);
    }

    public int CalculatePayout(int streak)
    {
        if (streak <= 0) return 0;

        const int basePayout = 10;
        const double baseFactor = 1.7;

        return (int)Math.Round(basePayout * Math.Pow(baseFactor, streak));
    }
}
