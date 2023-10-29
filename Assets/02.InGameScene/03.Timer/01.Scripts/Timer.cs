using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoSington<Timer>
{
    private float startTime;
    private float leftTime = 0;

    public Action onTimeOver;

    public void TimerInit()
    {
        leftTime = startTime;
    }

    public void TimerUpdate()
    {
        leftTime -= Time.deltaTime;
    }

    private void TimeOver()
    {

    }
}
