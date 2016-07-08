using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : Singleton<GameTimer>
{
    public float CurrentTime = -1;
    public Text TimerUI;
    public float StartTimerAlpha = 10.0f;
    [Range(1,5)]
    public int PulsesPerSecond = 2;

    private string text;

    // Use this for initialization
    void Start()
    {
        GameState.Instance.EventHookups.OnGameOver.AddListener(StopRound);
    }

    public void StartRound()
    {
        Debug.Log("StartRound");
        text = "TIME ";
        StartCoroutine(RoundTimer());
    }

    public void StopRound()
    {
        Debug.Log("StopRound");
        StopAllCoroutines();
    }

    public IEnumerator Timer(float timerStartValue, bool IgnorePause = false, string timerText = "TIME ")
    {
        text = timerText;
        CurrentTime = timerStartValue;
        while (CurrentTime > 0)
        {
            if(IgnorePause || GameState.Instance.State != GameState.GameStateEnum.Pause)
                CurrentTime -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RoundTimer()
    {
        yield return StartCoroutine(Timer(GameLogic.Instance.Rules.RoundTime));

        // Speak to gamelogic for round finished
        GameLogic.Instance.CheckForGameOver();
    }

    public void Update()
    {
        TimerUI.text = text + (int)CurrentTime;
    }
}
