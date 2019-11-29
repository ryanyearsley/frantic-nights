using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEventManager : GameManager
{
    //instantiates
    public GameObject timeWidget;
    public GameObject splitUI;

    //static vars
    [SerializeField]
    public float timeToBeatSeconds;
    public float goldTime;
    public float silverTime;
    public float bronzeTime;
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

    private List<Text> bestSplitsTexts = new List<Text>();
    private List<Text> currentSplitsTexts = new List<Text>();




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

        for (int i = 0; i < splitCount; i++)
        {
            GameObject go = Instantiate(splitUI, widget.transform);

            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.position += new Vector3(0, (i *-25), 0);
            bestSplitsTexts.Add(go.transform.Find("BestSplit").GetComponent<Text>());
            currentSplitsTexts.Add(go.transform.Find("CurrentSplit").GetComponent<Text>());
            go.transform.Find("SplitNumber").GetComponent<Text>().text = i.ToString();
        }


        disableUnusedSplits();
        clearCurrentSplits();
    }
    private void disableUnusedSplits()
    {
       for (int i = 0; i < bestSplitsTexts.Count; i++)
        {
            if (i > splitCount)
                bestSplitsTexts[i].enabled = false;
        }
        for (int i = 0; i < currentSplitsTexts.Count; i++)
        {
            if (i > splitCount)
                currentSplitsTexts[i].enabled = false;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (runActive)
        {
            currentTimeSeconds += Time.deltaTime;
            currentTimeTxt.text = TimeUtility.convertFloatToTimeString(currentTimeSeconds);
        }
    }

    public void startAttempt()
    {
        clearCurrentSplits();
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
                transmitGameMessage("WINNER!", "Press F to pay respects.", 3);
            }
            else
                transmitGameMessage("LOSER!", 3);

            if (bestAttempt == null)
                updateBestAttempt(lastAttempt);

            updateBestAttempt(TimeUtility.compareTimesForBest(allAttempts, bestAttempt));
        }
        else
        {
            transmitGameMessage("INVALID TIME!", 3);
        }
        return;
    }

    private void updateBestAttempt(LapTime newBestAttempt)
    {
        bestAttempt = newBestAttempt;
        bestAttemptTxt.text = TimeUtility.convertFloatToTimeString(bestAttempt.time);
        updateBestSplits(bestAttempt);
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
        updateCurrentSplit(currentAttempt.splitIndex, currentTimeSeconds);

        //update splits UI
    }
    private void updateCurrentSplit(int splitIndex, float time)
    {
        Text currentSplitText = currentSplitsTexts[splitIndex - 1];
        currentSplitText.text = TimeUtility.convertFloatToTimeString(time);
        if (bestAttempt != null)
        {
            if (bestAttempt.splits[splitIndex - 1] < time)
                currentSplitText.color = Color.red;
            else
                currentSplitText.color = Color.green;
        }

    }

    private void updateBestSplits(LapTime lap)
    {
        for(int i = 0; i < lap.splits.Count; i++)
        {
        bestSplitsTexts[i].text = TimeUtility.convertFloatToTimeString(lap.splits[i]);
        }
    }

    private void clearCurrentSplits()
    {
        for (int i = 0; i < currentSplitsTexts.Count; i++)
        {
            currentSplitsTexts[i].text = "-";
            currentSplitsTexts[i].color = Color.white;
        }
    }



}
