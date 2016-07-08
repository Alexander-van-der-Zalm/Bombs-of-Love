using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

public class GameLogic : Singleton<GameLogic>
{
    #region Fields

    public List<Player> Players;

    public Player Winner;
    public LevelRules Rules;

    private GameTimer timer;

    #endregion

    #region Awake

    public void Awake()
    {
        instance = this;
        //GameState.Instance.EventHookups.OnGameStart.AddListener(NewRound);
    }

    #endregion

    #region CheckGameOver

    public void CheckForGameOver()
    {
        // Check if only one player has positive lives
        int playerAlive = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Lives > 0)
                playerAlive++;
        }
        if (playerAlive == 1)
        {
            Winner = Players.Where(p => p.Lives > 0).First();
            GameState.Instance.GameOverState = GameState.GameOverStates.Win;
            RoundFinished();
            return;
        }
        if (playerAlive == 0)
        {
            GameState.Instance.GameOverState = GameState.GameOverStates.Draw;
            RoundFinished();
            return;
        }
        if (GameTimer.Instance.CurrentTime <= 0)
        {
            //if (Rules.SuddenDeathOnTimeUp)
            //{
            //    StartSuddenDeath();
            //    return;
            //}

            if (Rules.LoseOnTimeUp)
                GameState.Instance.GameOverState = GameState.GameOverStates.Lose;
            else
                GameState.Instance.GameOverState = GameState.GameOverStates.Draw;
            RoundFinished();
            return;
        }
    }

    #endregion

    #region RoundFinished

    public void RoundFinished()
    {
        StartCoroutine(RoundFinishedCR());
    }

    private IEnumerator RoundFinishedCR()
    {
        GameState.Instance.State = GameState.GameStateEnum.Pause;
        yield return new WaitForSeconds(3.0f);
        // Audio for finished
        Debug.Log("RoundFinishedCR");
        //// Cleanup old stuff
        //DeleteAll(typeof(Bomb));
        //DeleteAll(typeof(Explosion));
        //DeleteAll(typeof(PowerUp));

        GameState.Instance.GameOver();
    }

    #endregion

    private void DeleteAll(Type type)
    {
        UnityEngine.Object[] objs = GameObject.FindObjectsOfType(type);
        int objsCount = objs.Length;
        for(int i = 0; i < objsCount; i++)
        {
            GameObject.Destroy(objs[i]);
        }
    }

    

    #region New Round

    public void NewRound()
    {
        foreach(Player player in Players)
        {
            player.Lives = Rules.Lives;
        }
        // Reset upgrade
        // Reset Grid??

        StartCoroutine(NewRoundCR());
    }

    private IEnumerator NewRoundCR()
    {
        GameState.Instance.State = GameState.GameStateEnum.Pause;
        Debug.Log("GL - StartCOuntDown ");

        yield return GameTimer.Instance.Timer(4.0f, true, "START IN ");
        // Audio For Start

        GameState.Instance.State = GameState.GameStateEnum.Play;
        GameTimer.Instance.StartRound();
    }

    #endregion
}
