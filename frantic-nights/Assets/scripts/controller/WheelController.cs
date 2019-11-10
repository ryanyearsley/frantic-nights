using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public WheelCollider wheel;
    private RCC_Skidmarks skidmarks;
    private float totalSlip;
    public float startSlipValue = .55f;
    private int lastSkidmark = -1;
    public bool receivingPower = false;
    public bool receivingTurnInput = false;
    public bool rpmSensorWheel = false;
    public GameObject wheelShapePrefab;
    private GameObject wheelShape;

    private void Start()
    {
        wheel = gameObject.GetComponent<WheelCollider>();
        skidmarks = GameObject.FindObjectOfType(typeof(RCC_Skidmarks)) as RCC_Skidmarks;
    }

    public void initializeWheel(DriveType driveType)
    {
        if (wheelShape != null)
        {
            var ws = Instantiate(wheelShape);
            ws.transform.parent = wheel.transform;
        }
        if (transform.localPosition.z > 0)
        {
            receivingTurnInput = true;
        }
        switch (driveType)
        {
            case DriveType.FrontWheelDrive:
                //front wheels
                if (transform.localPosition.z > 0)
                {
                    receivingPower = true;
                }
                break;
            case DriveType.RearWheelDrive:
                //rear wheels
                if (transform.localPosition.z < 0)
                {
                    receivingPower = true;
                }
                break;
            case DriveType.AllWheelDrive:
                receivingPower = true;
                break;
        }
    }
    public void fixedUpdateWheelPhysics(PlayerInputs pi, VehicleWheelMessage vehicleWheelMessage) 
    {
        //turn
        if (receivingTurnInput)
        {
            wheel.steerAngle = vehicleWheelMessage.currentAngle;
        }

        //power
        if (receivingPower && !vehicleWheelMessage.isRedlined)
        {
            wheel.motorTorque = vehicleWheelMessage.currentTorque;
        }
        else
            wheel.motorTorque = 0;

        //braking
        wheel.brakeTorque = pi.brakeInput;
        if (!receivingTurnInput && pi.handBrakeButton)
            wheel.brakeTorque = 10000f;
    }

    public void calculateWheelMeshPositions()
    {
        Quaternion q;
        Vector3 p;
        wheel.GetWorldPose(out p, out q);
        // Assume that the only child of the wheelcollider is the wheel shape.
        Transform shapeTransform = wheel.transform.GetChild(0);
        shapeTransform.position = p;
        shapeTransform.rotation = q;
    }

    public void generateSkidmarks(Vector3 velocity, float magnitude)
    {

        // First, we are getting groundhit data.
        WheelHit GroundHit;
        wheel.GetGroundHit(out GroundHit);

        // Forward, sideways, and total slips.
        float wheelSlipAmountForward = Mathf.Abs(GroundHit.forwardSlip);
        float wheelSlipAmountSideways = Mathf.Abs(GroundHit.sidewaysSlip);

        totalSlip = Mathf.Lerp(totalSlip, (wheelSlipAmountSideways + wheelSlipAmountForward), Time.fixedDeltaTime * 3f) / 1f;

        // If scene has skidmarks manager...
        if (skidmarks)
        {
            // If slips are bigger than target value...
            if (wheelSlipAmountSideways + wheelSlipAmountForward > startSlipValue)
            {

                Vector3 skidPoint = GroundHit.point + 2f * (velocity) * Time.deltaTime;

                if (magnitude > 1f)
                    lastSkidmark = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, totalSlip, lastSkidmark);
                else
                    lastSkidmark = -1;

            }
            else
            {
                lastSkidmark = -1;
            }

        }

    }

}
