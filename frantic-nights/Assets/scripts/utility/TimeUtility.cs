using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class TimeUtility : MonoBehaviour
{
    public static bool validateTime (LapTime attempt, int splitCount)
    {
        if (attempt.splitIndex == splitCount)
            return true;
        else return false;
    }

    public static string convertFloatToTimeString(float inputTime)
    {
        var ts = TimeSpan.FromSeconds(inputTime);
        return string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

    }
    public static LapTime compareTimesForBest(List<LapTime> allAttempts, LapTime currentBestTime)
    {
        allAttempts.Sort((x, y) => x.time.CompareTo(y.time));

        LapTime bestTime = allAttempts[0];
        if (currentBestTime != null)
        {
            bestTime = currentBestTime;
        }
        else
        {
            bestTime.time = 600;
        }
        foreach (LapTime attempt in allAttempts)
        {
            float time = attempt.time;
            if (time < bestTime.time)
            {
                bestTime = attempt;
            }
        }
        return bestTime;
    }
}


