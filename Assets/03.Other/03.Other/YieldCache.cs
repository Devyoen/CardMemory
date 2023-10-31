using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> waitForSecondsDict = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float time)
    {
        if (waitForSecondsDict.ContainsKey(time))
            return waitForSecondsDict[time];
        else
        {
            WaitForSeconds wait = new WaitForSeconds(time);
            waitForSecondsDict.Add(time, wait);
            return wait;
        }
    }
}
