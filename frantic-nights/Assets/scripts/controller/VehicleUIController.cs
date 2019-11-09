using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUIController : MonoBehaviour
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
    public void updateGearUI(int gear, float accelTest)
    {
        if (gear == 0)
            gearDisplay.text = "R";
        else
            gearDisplay.text = gear.ToString();

        tachNeedle.rotation = Quaternion.Euler(180, 0, accelTest * 200f);
    }

    public void updateUI(float currentSpeed, float currentRpm)
    {
        kphDisplay.text = Mathf.RoundToInt(currentSpeed) + " k/h";

        tachNeedle.rotation = Quaternion.Euler(180, 0, currentRpm * 200f);
    }
}
