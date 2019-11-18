using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeTrialManager : GameManager
{
    public GameObject startTrigger;
    public GameObject endTrigger;

    public GameObject timeWidget;

    public bool runActive;

    public float timeToBeat;
    private float currentTime;
    private float bestAttempt = 900;
    private float lastAttempt;

    public List<float> allAttempts = new List<float>();

    private Text timeToBeatTxt;
    private Text currentTimeTxt;
    private Text bestAttemptTxt;
    private Text lastAttemptTxt;
    // Start is called before the first frame update
    protected override void initializeGameManager()
    {
        base.initializeGameManager();
        GameObject widget = Instantiate(timeWidget, GameObject.FindObjectOfType<Canvas>().transform);
        timeToBeatTxt = widget.transform.Find("TimeToBeat").gameObject.GetComponent<Text>();
        currentTimeTxt = widget.transform.Find("CurrentTime").gameObject.GetComponent<Text>();
        bestAttemptTxt = widget.transform.Find("BestAttempt").gameObject.GetComponent<Text>();
        lastAttemptTxt = widget.transform.Find("LastAttempt").gameObject.GetComponent<Text>();
        timeToBeatTxt.text = TimeUtility.convertFloatToTimeString(timeToBeat);
        currentTimeTxt.text = "-";
        bestAttemptTxt.text = TimeUtility.convertFloatToTimeString(bestAttempt);
        lastAttemptTxt.text = TimeUtility.convertFloatToTimeString(0f);
        allAttempts.Add(bestAttempt);
    }

    // Update is called once per frame
    void Update()
    {
        if (runActive)
        {
            currentTime += Time.deltaTime;
            currentTimeTxt.text = TimeUtility.convertFloatToTimeString(currentTime);
        }
    }

    public void startTimeTrialEvent()
    {
        runActive = true;

    }
    public void endTimeTrialEvent()
    {
        runActive = false;
        submitTime(currentTime);
        resetObjective();
    }
    public void submitTime(float time)
    {
        lastAttempt = time;
        lastAttemptTxt.text = TimeUtility.convertFloatToTimeString(lastAttempt);
        allAttempts.Add(lastAttempt);
        bestAttempt = TimeUtility.compareTimesForBest(allAttempts, bestAttempt);
        bestAttemptTxt.text = TimeUtility.convertFloatToTimeString(bestAttempt);
        if (lastAttempt < timeToBeat)
        {
            transmitGameMessage("WINNER!", 3);
        }
        else
            transmitGameMessage("LOSER!", 3);
    }

    public override void resetObjective()
    {
        print("TT manager reset");
        currentTime = 0;
        currentTimeTxt.text = TimeUtility.convertFloatToTimeString(currentTime);
        runActive = false;
    }
}
