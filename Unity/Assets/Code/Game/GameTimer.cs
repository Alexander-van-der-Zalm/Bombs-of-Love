using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : Singleton<GameTimer>
{
    public float RoundTime = 59.0f;
    public float CurrentTime = -1;
    public Text TimerUI;
    public float StartTimerAlpha = 10.0f;
    [Range(1,5)]
    public int PulsesPerSecond = 2;

    // Use this for initialization
    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        StartCoroutine(Timer());
        // Subscribe to GameOver
        GameState.Instance.EventHookups.OnGameOver.AddListener(StopRound);
    }

    public void StopRound()
    {
        StopAllCoroutines();
    }

    private IEnumerator Timer()
    {
        CurrentTime = RoundTime;
        while (CurrentTime > 0)
        {

            CurrentTime -= Time.deltaTime;
            yield return null;
        }

        // Speak to gamelogic for game finished
        GameLogic.Instance.CheckForGameOver();
    }

    public void Update()
    {
        TimerUI.text = "TIME " + (int)CurrentTime;
        //if(CurrentTime < 10.0f)
        //{
        //    float alpha = (CurrentTime - (int)CurrentTime)
        //    TimerUI.Col
        //}
    }
}
