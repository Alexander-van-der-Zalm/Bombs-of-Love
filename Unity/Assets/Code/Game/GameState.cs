using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameState : Singleton<GameState>
{
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

    public GameStateEnum State;
    public GameOverStates GameOverState = GameOverStates.Win;

    public UnityEvent OnGameOver;
    public UnityEvent OnGameStart;
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;

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

        OnGamePaused.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = oldTimeScale;
        State = GameStateEnum.Play;
        Debug.Log("ResumeGame");

        // Call other functions
        OnGameResumed.Invoke();
    }

    public void StartGame()
    {
        State = GameStateEnum.Play;
        Debug.Log("StartGame");

        OnGameStart.Invoke();
    }

    public void GameOver()
    {
        State = GameStateEnum.Menu;
        Debug.Log("GameOver");

        OnGameOver.Invoke();
    }
}
