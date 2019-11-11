using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class VehicleController : MonoBehaviour
{
    VehicleAudioController audioController;
    VehicleUIController vehicleUIController;
    private PhysicsController physicsController;

    private Rigidbody rb;


    public VehicleWheelMessage currentVehicle;

    //currents
    [SerializeField]
    private int currentGear;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float currentTorque;
    [SerializeField]
    private float currentEngineRpm;
    [SerializeField]
    private float currentBrake;
    [SerializeField]
    public float currentSteeringAngle = 0f;

    private int currentFrontWheelRpm;
    private int currentRearWheelRpm;

    //Maximums
    public float maxTorque = 3000f;
    public float maxEngineRpm = 7000f;
    public float maxBrakeTorque = 3000f;
    public float maxHandbrakeTorque = 10000f;
    public float maxSteeringAngle = 30f;

    //Steering
    public float steerSmooth = 0.1f;
    private float baseSteerSmooth = 0.1f;
    private float steerVelocity = 0.1f;

    //Engine
    public bool isRedlined = false;

    //Wheels
    private WheelCollider[] wheelColliders;
    private WheelController[] wheelControllers;
    public float startSlipValue = .35f;


    //Transmission
    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    public Gear[] gears;
    public bool isReverse;

    //Wheel Physics Sub-steps
    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    private float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    private int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    private int stepsAbove = 1;

    //Visual
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponentInChildren<VehicleAudioController>();
        physicsController = GetComponentInChildren<PhysicsController>();
        vehicleUIController = GetComponentInChildren<VehicleUIController>();
        rb = GetComponent<Rigidbody>();
        baseSteerSmooth = steerSmooth;
        Mathf.Clamp(currentGear, 0, 5);
        currentGear = 1;

        wheelControllers = GetComponentsInChildren<WheelController>();

        foreach (WheelController wheelController in wheelControllers)
        {
            print("Wheel " + wheelController.gameObject.name);
            wheelController.initializeWheel(driveType);
            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheelController.transform;
            }
        }
    }
    public void updateVehicle(PlayerInputs pi)
    {
        if (pi.gearUpButtonDown && currentGear < gears.Length - 1)
            changeGear(1);

        else if (pi.gearDownButtonDown && currentGear > 0)
            changeGear(-1);

        vehicleUIController.updateUI(currentSpeed, currentEngineRpm);
        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.generateSkidmarks(rb.velocity, rb.velocity.magnitude);
            wheelController.calculateWheelMeshPositions();
        }

        audioController.updateVehicleAudio(currentEngineRpm, pi.accelInput);
    }

    public void fixedUpdateVehicle(PlayerInputs pi)
    {
        

        VehicleWheelMessage vehicleWheelMessage = calculateVehiclePhysics(pi);
        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.fixedUpdateWheelPhysics(pi, vehicleWheelMessage);
            wheelController.wheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
        }
        physicsController.fixedUpdatePhysics(pi, vehicleWheelMessage);
    }

    private void changeGear(int gearChangeDir)
    {
        currentGear += gearChangeDir;
        vehicleUIController.updateGearUI(currentGear, currentEngineRpm);
    }

    private VehicleWheelMessage calculateVehiclePhysics(PlayerInputs pi)
    {
        float inputAngle = maxSteeringAngle * pi.steeringInput;
        steerSmooth = baseSteerSmooth + (currentSpeed * 0.0008f);
        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, inputAngle, ref steerVelocity, steerSmooth);


        //km/h
        currentSpeed = rb.velocity.magnitude * 3.6f;
        float speedPercentageToGearMax = currentSpeed / gears[currentGear].topSpeed;
        if (speedPercentageToGearMax > 1)
        {
            isRedlined = true;
        }
        else
            isRedlined = false;
        float gearEvaluation = (gears[currentGear].torqueCurve.Evaluate(speedPercentageToGearMax));
        //print("Gear eval: " + gearEvaluation);
        currentTorque = pi.accelInput * (maxTorque * gearEvaluation);

        //for UI use only
        currentEngineRpm = (speedPercentageToGearMax * maxEngineRpm) + 500;

        return generateVehicleMessage();
    }

    private VehicleWheelMessage generateVehicleMessage()
    {

        VehicleWheelMessage vwm = new VehicleWheelMessage();
        vwm.currentSpeed = currentSpeed;
        vwm.currentAngle = currentSteeringAngle;
        vwm.currentTorque = currentTorque;
        vwm.isRedlined = isRedlined;
        return vwm;
    }
}
