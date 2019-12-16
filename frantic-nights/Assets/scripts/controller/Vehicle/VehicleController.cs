using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class VehicleController : MonoBehaviour
{
    Vehicle vehicle;
    VehicleAudioController audioController;
    VehicleUIController vehicleUIController;
    VehicleVisualsController visualsController;
    private PhysicsController physicsController;

    private Rigidbody rb;


    public VehicleWheelMessage currentVehicle;

    //currents
    [SerializeField]
    private int currentGear;
    [SerializeField]
    private float currentSpeed;

    private float currentFlywheelVelocity;
    [SerializeField]
    private float currentTorque;
    [SerializeField]
    private float currentEngineRpm;
    [SerializeField]
    private float currentBrake;
    [SerializeField]
    public float currentSteeringAngle = 0f;

    private bool isShifting;
    [SerializeField]
    private float currentFrontWheelRpm;
    [SerializeField]
    private float currentRearWheelRpm;

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
    private float wheelDiameter = 2.198f;

    //Engine
    public bool isRedlined = false;
    public float engineVelocity = 0f;
    public float engineSmooth = 0.3f;
    public static float startSlipValue = .35f;

    //Wheels
    private WheelCollider[] wheelColliders;
    private WheelController[] wheelControllers;


    //Transmission
    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    public Gear[] gears;
    public bool isReverse;
    public static float shiftTime = 0.1f;

    //Wheel Physics Sub-steps
    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    private float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    private static int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    private static int stepsAbove = 1;

    //Visual
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponentInChildren<VehicleAudioController>();
        physicsController = GetComponentInChildren<PhysicsController>();
        vehicleUIController = GetComponentInChildren<VehicleUIController>();
        visualsController = GetComponentInChildren<VehicleVisualsController>();
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

        initializeGears();
        visualsController.initializeVisualsController();
        audioController.initializeAudio();

    }

    public void initializeGears()
    {
        foreach (Gear gear in gears)
        {
            //kmh CONVERT TO meters per second 
            //1 k/h = 3.6 m/s
            //example: second gear has a max RPM of 1k
            float topMeterPerSecond = gear.topSpeed / 3.6f;
            float topMeterPerMinute = topMeterPerSecond * 60f;

            gear.topWheelRpm = topMeterPerMinute / wheelDiameter;
        }
    }

    public void resetVehicle(Vector3 resetPosition, Quaternion resetLocation)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = resetPosition;
        transform.rotation = resetLocation;
        StartCoroutine(changeGear(1));
    }

    public void updateVehicle(PlayerInputs pi)
    {
        

        vehicleUIController.updateUI(currentSpeed, currentEngineRpm);
        foreach (WheelController wheelController in wheelControllers)
        {
            wheelController.generateSkidmarks(rb.velocity, rb.velocity.magnitude);
            wheelController.calculateWheelMeshPositions();
        }

        audioController.updateVehicleAudio(currentEngineRpm, pi.accelInput);
        visualsController.updateVehicleVisuals(pi);
    }

    public void fixedUpdateVehicle(PlayerInputs pi)
    {
        if (!isShifting)
        {
            if (pi.gearUpButtonDown && currentGear < gears.Length - 1)
                StartCoroutine(changeGear(currentGear += 1));

            else if (pi.gearDownButtonDown && currentGear > 0)
                StartCoroutine(changeGear(currentGear -= 1));
        }

        VehicleWheelMessage vehicleWheelMessage = calculateVehiclePhysics(pi);
        ApplyWheelPhysics(pi, vehicleWheelMessage);
        physicsController.fixedUpdatePhysics(pi, vehicleWheelMessage);
    }
    private void ApplyWheelPhysics(PlayerInputs pi, VehicleWheelMessage vehicleWheelMessage)
    {
        currentFrontWheelRpm = 0;
        currentRearWheelRpm = 0;
        foreach (WheelController wheelController in wheelControllers)
        {
            WheelVehicleMessage wvm = wheelController.fixedUpdateWheelPhysics(pi, vehicleWheelMessage);
            currentFrontWheelRpm += wvm.frontWheelRpm;
            currentRearWheelRpm += wvm.rearWheelRpm;
            wheelController.wheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
        }
        currentFrontWheelRpm /= 2;
        currentRearWheelRpm /= 2;
    }

    private IEnumerator changeGear(int newGear)
    {
        isShifting = true;
        yield return new WaitForSeconds(shiftTime);
         currentGear = newGear;
        vehicleUIController.updateGearUI(currentGear, currentEngineRpm);
        isShifting = false;
    }


    private VehicleWheelMessage calculateVehiclePhysics(PlayerInputs pi)
    {
        //STOP GO TURN
        //meters per second >> kph
        currentSpeed = rb.velocity.magnitude * 3.6f;
        calculateEngineOutputArcade(pi.accelInput);
        calculateBrakeOutput(pi.brakeInput);
        calculateSteeringAngle(pi.steeringInput);
        return generateVehicleMessage();
    }

    private void calculateEngineOutputArcade(float accelInput)
    {
        currentSpeed = rb.velocity.magnitude * 3.6f;
        float speedPercentageToGearMax = currentSpeed / gears[currentGear].topSpeed;
        if (speedPercentageToGearMax > 1)
        {
            isRedlined = true;
        }
        else
            isRedlined = false;

        float gearEvaluation = (gears[currentGear].torqueCurve.Evaluate((speedPercentageToGearMax)));
        currentTorque = accelInput * (maxTorque * gearEvaluation);
        currentEngineRpm = (speedPercentageToGearMax * maxEngineRpm) + 500;
    }

    /*
    private void calculateEngineOutputSim(float accelInput)
    {
        currentSpeed = rb.velocity.magnitude * 3.6f;
        //0-1  value

        float speedPercentageToGearMax = currentSpeed / gears[currentGear].topSpeed;
        float speedPercentageToGearMaxWheel = currentRearWheelRpm / gears[currentGear].topWheelRpm;
        float averageSpeedPercentage = ((speedPercentageToGearMax * 5) + speedPercentageToGearMaxWheel) / 2;
        if (averageSpeedPercentage > 1)
        {
            isRedlined = true;
        }
        else
            isRedlined = false;
        
        float gearEvaluation = (gears[currentGear].torqueCurve.Evaluate(averageSpeedPercentage));
        //currentFlywheelVelocity = Mathf.SmoothDamp(currentFlywheelVelocity, gearEvaluation, ref engineVelocity, engineSmooth);


        currentTorque = accelInput * (maxTorque * gearEvaluation);
        currentEngineRpm = (averageSpeedPercentage * maxEngineRpm) + 500;
    }
    */

    private void calculateBrakeOutput(float brakeInput)
    {
        currentBrake = brakeInput * maxBrakeTorque;
    }

    private void calculateSteeringAngle(float steeringInput)
    {
        float inputAngle = maxSteeringAngle * steeringInput;
        steerSmooth = baseSteerSmooth + (currentSpeed * 0.0008f);
        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, inputAngle, ref steerVelocity, baseSteerSmooth);
    }

    private void calculateSteeringAngleRaw(float steeringInput)
    {
        currentSteeringAngle = maxSteeringAngle * steeringInput;
    }

        private VehicleWheelMessage generateVehicleMessage()
    {

        VehicleWheelMessage vwm = new VehicleWheelMessage();
        vwm.currentSpeed = currentSpeed;
        vwm.currentBrake = currentBrake;
        vwm.currentAngle = currentSteeringAngle;
        vwm.currentTorque = currentTorque;
        vwm.isRedlined = isRedlined;
        return vwm;
    }
}
