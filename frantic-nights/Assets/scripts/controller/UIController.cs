using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //UI
    public Text gearDisplay;
    public Text kphDisplay;
    public Text debugDisplay;
    public RectTransform tachNeedle;
    public float idleTachAngle;
    public float redlineTachAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void updateUI (float currentSpeed, float currentRpm)
    {
        kphDisplay.text = Mathf.RoundToInt(currentSpeed) + " k/h";
    }
}
