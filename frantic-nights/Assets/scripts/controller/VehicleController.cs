using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class VehicleController : MonoBehaviour
{
    VehicleUIController vehicleUIController;

    private Rigidbody rb;

    //currents
    private int currentGear;
    private float currentSpeed;
    private float currentTorque;
    private float currentBrake;
    public float currentSteeringAngle = 0f;
    private int currentFrontWheelRpm;
    private int currentRearWheelRpm;

    //Steering
    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 30f;
    public float steerSmooth = 0.1f;
    private float baseSteerSmooth = 0.1f;
    private float steerVelocity = 0.0F;

    //Engine
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 1000f;
    public float accelTest;
    public bool isRedlined = false;

    //Braking
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    public float handbrakeTorque = 50000f;

    //Wheels
    private WheelCollider[] wheelColliders;
    private WheelController[] wheelControllers;


    //Transmission
    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    public AnimationCurve[] torqueCurves;
    public bool isReverse;
    public int[] topSpeed;
    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    //Visual
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    // Start is called before the first frame update
    void Start()
    {
        vehicleUIController = GetComponent<VehicleUIController>();
        rb = GetComponent<Rigidbody>();
        baseSteerSmooth = steerSmooth;
        Mathf.Clamp(currentGear, 0, 5);
        currentGear = 1;

        wheelControllers = GetComponentsInChildren<WheelController>();

        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.initializeWheel(driveType);
            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheelController.transform;
            }
        }
    }
    public void updateVehicle()
    {
        vehicleUIController.updateUI(currentSpeed, accelTest);
        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.generateSkidmarks(rb.velocity, rb.velocity.magnitude);
            wheelController.calculateWheelMeshPositions();
        }
    }

    public void fixedUpdateVehicle(PlayerInputs pi)
    {
        if (pi.gearUpButtonDown && currentGear < topSpeed.Length - 1)
            currentGear++;

        else if (pi.gearDownButtonDown && currentGear > 0)
            currentGear--;

        vehicleUIController.updateGearUI(currentGear, accelTest);


        VehicleWheelMessage vehicleWheelMessage = calculateVehiclePhysics(pi);
        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.fixedUpdateWheelPhysics(vehicleWheelMessage);
            wheelController.wheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
        }
    }

    private VehicleWheelMessage calculateVehiclePhysics(PlayerInputs pi)
    {
        float inputAngle = maxAngle * pi.steeringInput;
        steerSmooth = baseSteerSmooth + (currentSpeed * 0.0008f);
        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, inputAngle, ref steerVelocity, steerSmooth);


        currentSpeed = rb.velocity.magnitude * 3.6f;
        currentTorque = (torqueCurves[currentGear].Evaluate(currentSpeed / topSpeed[currentGear]));
        accelTest = (currentSpeed / topSpeed[currentGear]);
        currentTorque = pi.accelInput * (maxTorque * currentTorque);


        VehicleWheelMessage vwm = new VehicleWheelMessage();
        vwm.currentAngle = currentSteeringAngle;
        vwm.currentTorque = currentTorque;
        vwm.currentBrake = pi.brakeInput;
        vwm.isHandbraking = pi.handBrakeButton;
        vwm.isRedlined = isRedlined;
        return vwm;

    }
}
