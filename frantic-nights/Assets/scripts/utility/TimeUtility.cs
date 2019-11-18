using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class TimeUtility : MonoBehaviour
{

    public static string convertFloatToTimeString(float inputTime)
    {
        var ts = TimeSpan.FromSeconds(inputTime);
        return string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

    }
    public static float compareTimesForBest(List<float> allAttempts, float currentBestTime)
    {
        float bestTime = currentBestTime;
        foreach (float attempt in allAttempts)
        {
            if (attempt < bestTime)
            {
                bestTime = attempt;
            }
        }
        return bestTime;
    }
}


