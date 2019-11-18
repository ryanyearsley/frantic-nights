using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialTrigger : MonoBehaviour
{
    private TimeTrialManager timeTrialManager;

    public bool isStart;


    // Start is called before the first frame update
    void Start()
    {
        timeTrialManager = FindObjectOfType<TimeTrialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("trigger hit: " + other.tag);
        if (other.tag == "Player")
        {
            if (isStart)
            {
                GameManager.instance.resetObjective();
                timeTrialManager.startTimeTrialEvent();
            }
            else
                timeTrialManager.endTimeTrialEvent();
        }
    }
}
