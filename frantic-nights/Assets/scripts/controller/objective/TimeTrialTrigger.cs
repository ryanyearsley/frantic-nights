using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialTrigger : MonoBehaviour
{
    public bool isStart;
    private TimedEventManager timeTrialManager;

    void Start()
    {
        timeTrialManager = FindObjectOfType<TimedEventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("trigger hit: " + other.tag);
        if (other.tag == "Player")
        {
            if (isStart)
            {
                GameManager.instance.resetObjective();
                timeTrialManager.startAttempt();
            }
            else
                timeTrialManager.endAttempt();
        }
    }
}
