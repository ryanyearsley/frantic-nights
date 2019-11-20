using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitTrigger : MonoBehaviour
{
    public int splitIndex;
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
            timeTrialManager.checkpoint(splitIndex);
        }
    }
}
