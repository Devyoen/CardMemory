using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoSington<Timer>
{
    private float startTime;
    private float leftTime = 0;
    public float LeftTime => leftTime;

    public Action onTimeOver;

    public void TimerInit()
    {
        startTime = InGameManager.instance.GetDifficultyData().Time;
        leftTime = startTime;
    }

    public void TimerUpdate()
    {
        leftTime -= Time.deltaTime;
        leftTime = Mathf.Clamp(leftTime, 0, startTime);
        if (leftTime <= 0)
            TimeOver();
        InGameUIs.instance.UpdateUI(UIType.Timer, Timer.instance.LeftTime);
    }

    private void TimeOver()
    {
        onTimeOver?.Invoke();
    }
}
