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

    [Range(-127, 28)]
    private float currentTachAngle = 0f;
    //Z-rot of UI tachNeedle element at 0 RPM
    private float idleTachAngle = 27.38f;
    //Z-rot of UI tachNeedle element at 9000 RPM (Redline)
    private float redlineTachAngle = -246.84f;

    private float tachRotPerRPM = 32.8f;

    private float tachOffsetRot = 27.772f;

    //274.2 degrees between 0-9k
    //  9,000  /       -274.2    =            -32.8
    // redline /  tach range rot = tach rotation degree per RPM
    void Start()
    {
        //tachRotPerRPM = (redlineTachAngle - idleTachAngle) / 9000;
    }
    public void updateGearUI(int gear, float accelTest)
    {
        if (gear == 0)
            gearDisplay.text = "R";
        else
            gearDisplay.text = gear.ToString();
    }

    public void updateUI(float currentSpeed, float currentRpm)
    {
        kphDisplay.text = Mathf.RoundToInt(currentSpeed) + " k/h";
        debugDisplay.text = currentRpm.ToString();
        currentTachAngle = (currentRpm / tachRotPerRPM) - tachOffsetRot;
        Mathf.Clamp(currentTachAngle, redlineTachAngle, idleTachAngle);
        tachNeedle.rotation = Quaternion.Euler(180, 0, currentTachAngle);
    }
}
