using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameState : Singleton<GameState>
{
    #region Enums & Classes

    public enum GameStateEnum
    {
        Menu,
        Pause,
        Play
    }

    public enum GameOverStates
    {
        Win,
        Draw,
        Lose
    }

    [System.Serializable]
    public class GameStateEventHookups
    {
        public UnityEvent OnGameOver;
        public UnityEvent OnGameStart;
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;
    }

    #endregion

    public GameStateEnum State;
    public GameOverStates GameOverState = GameOverStates.Win;
    public GameStateEventHookups EventHookups;

    private float oldTimeScale = -1;

    public void Awake()
    {
        instance = this;
    }

    public void PauseGame()
    {
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0;
        State = GameStateEnum.Pause;
        Debug.Log("PauseGame");

        EventHookups.OnGamePaused.Invoke();
    }

    public void ResumeGame()
    {
        if(oldTimeScale >= 0)
            Time.timeScale = oldTimeScale;
        State = GameStateEnum.Play;
        Debug.Log("ResumeGame");

        // Call other functions
        EventHookups.OnGameResumed.Invoke();
    }

    public void StartGame()
    {
        State = GameStateEnum.Play;
        Debug.Log("StartGame");

        EventHookups.OnGameStart.Invoke();
    }

    public void GameOver()
    {
        State = GameStateEnum.Menu;
        Debug.Log("GameOver");

        EventHookups.OnGameOver.Invoke();

        switch(GameOverState)
        {
            default:
            case GameOverStates.Win:
                GameMenu.Instance.GameWon();
                return;
            case GameOverStates.Draw:
                GameMenu.Instance.GameDraw();
                return;
            case GameOverStates.Lose:
                GameMenu.Instance.GameLost();
                return;
        }
    }
}
