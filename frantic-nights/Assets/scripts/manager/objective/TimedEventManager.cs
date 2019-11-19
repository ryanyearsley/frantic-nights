using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEventManager : GameManager
{
    //instantiates
    public GameObject timeWidget;

    //static vars
    [SerializeField]
    public float timeToBeatSeconds;
    public static float defaultBestTimeSeconds = 600f; //always 15 mins

    //dynamic vars
    public bool runActive;
    public float currentTimeSeconds;
    private int splitCount;

    public List<LapTime> allAttempts = new List<LapTime>();

    public LapTime currentAttempt;
    public LapTime lastAttempt;
    public LapTime bestAttempt;

    //UI
    private Text timeToBeatTxt;
    private Text currentTimeTxt;
    private Text bestAttemptTxt;
    private Text lastAttemptTxt;




    protected override void initializeGameManager()
    {
        base.initializeGameManager();
        GameObject widget = Instantiate(timeWidget, GameObject.FindObjectOfType<Canvas>().transform);
        timeToBeatTxt = widget.transform.Find("TimeToBeat").gameObject.GetComponent<Text>();
        currentTimeTxt = widget.transform.Find("CurrentTime").gameObject.GetComponent<Text>();
        bestAttemptTxt = widget.transform.Find("BestAttempt").gameObject.GetComponent<Text>();
        lastAttemptTxt = widget.transform.Find("LastAttempt").gameObject.GetComponent<Text>();
        timeToBeatTxt.text = TimeUtility.convertFloatToTimeString(timeToBeatSeconds);
        currentTimeTxt.text = "-";
        bestAttemptTxt.text = "-";
        lastAttemptTxt.text = "-";
        SplitTrigger[] splits = FindObjectsOfType<SplitTrigger>();
        print("splits: " + splits.Length);
        splitCount = splits.Length;

    }

    void Update()
    {
        if (runActive)
        {
            currentTimeSeconds += Time.deltaTime;
            currentTimeTxt.text = TimeUtility.convertFloatToTimeString(currentTimeSeconds);
        }
    }

    public void startAttempt()
    {
        runActive = true;
        currentTimeSeconds = 0;
        currentAttempt = new LapTime();
        currentAttempt.splitIndex = 0;
    }

    public void endAttempt()
    {
        if (runActive == true)
        {
            runActive = false;
            //verify they hit all the checkpoints
            submitCurrentTime();
        }
    }

    public void submitCurrentTime()
    {
        currentAttempt.time = currentTimeSeconds;
        bool isValidTime = TimeUtility.validateTime(currentAttempt, splitCount);
        if (isValidTime)
        {
            lastAttempt = currentAttempt;
            lastAttemptTxt.text = TimeUtility.convertFloatToTimeString(lastAttempt.time);
            allAttempts.Add(lastAttempt);
            if (lastAttempt.time < timeToBeatSeconds)
            {
                transmitGameMessage("WINNER!", 3);
            }
            else
                transmitGameMessage("LOSER!", 3);
            bestAttempt = TimeUtility.compareTimesForBest(allAttempts, bestAttempt);
            bestAttemptTxt.text = TimeUtility.convertFloatToTimeString(bestAttempt.time);
        }
        else
        {
            transmitGameMessage("INVALID TIME!", 3);
        }
        return;
    }

    //new lap
    public void cycleAttempt()
    {
        endAttempt();
        startAttempt();
    }


    public void checkpoint(int index)
    {
        //valid 
        if (currentAttempt.splitIndex == (index - 1))
        {
            currentAttempt.splitIndex += 1;
        }
        currentAttempt.splits.Add(currentTimeSeconds);
    }



}
