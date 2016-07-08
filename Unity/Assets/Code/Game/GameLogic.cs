using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class GameLogic : Singleton<GameLogic>
{
    public List<Player> Players;

    public Player Winner;
    public LevelRules Rules;

    private GameTimer timer;

    public void Awake()
    {
        instance = this;
    }

    public void CheckForGameOver()
    {
        // Check if only one player has positive lives
        int playerAlive = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Lives > 1)
                playerAlive++;
        }
        if (playerAlive == 1)
        {
            Winner = Players.Where(p => p.Lives > 0).First();
            GameState.Instance.GameOverState = GameState.GameOverStates.Win;
            GameState.Instance.GameOver();
            return;
        }
        if (playerAlive == 0)
        {
            GameState.Instance.GameOverState = GameState.GameOverStates.Draw;
            GameState.Instance.GameOver();
            return;
        }
    }

    public void RoundFinished()
    {
        if (Rules.LoseOnTimeUp)
            GameState.Instance.GameOverState = GameState.GameOverStates.Lose;
        else
            GameState.Instance.GameOverState = GameState.GameOverStates.Draw;
        
        //if (Rules.SuddenDeathOnTimeUp)
        //{
        //    StartSuddenDeath();
        //    return;
        //}

        GameState.Instance.GameOver();
    }
}
