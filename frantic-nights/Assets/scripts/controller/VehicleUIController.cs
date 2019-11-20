using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUIController : MonoBehaviour
{
    public GameObject vehicleDashWidget;

    //UI
    private Text gearText;
    private Text kphText;
    private Text rpmText;
    private RectTransform tachNeedle;


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

        GameObject widget = Instantiate(vehicleDashWidget, GameObject.FindObjectOfType<Canvas>().transform);
        gearText = widget.transform.Find("gearText").gameObject.GetComponent<Text>();
        kphText = widget.transform.Find("kphText").gameObject.GetComponent<Text>();
        rpmText = widget.transform.Find("rpmText").gameObject.GetComponent<Text>();
        tachNeedle = widget.transform.Find("tachNeedle").gameObject.GetComponent<RectTransform>();
        
    }
    public void updateGearUI(int gear, float accelTest)
    {
        if (gear == 0)
            gearText.text = "R";
        else
            gearText.text = gear.ToString();
    }

    public void updateUI(float currentSpeed, float currentRpm)
    {
        kphText.text = Mathf.RoundToInt(currentSpeed) + " k/h";
        rpmText.text = currentRpm.ToString();
        currentTachAngle = (currentRpm / tachRotPerRPM) - tachOffsetRot;
        Mathf.Clamp(currentTachAngle, redlineTachAngle, idleTachAngle);
        tachNeedle.rotation = Quaternion.Euler(180, 0, currentTachAngle);
    }

}
