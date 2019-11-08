using UnityEngine;
using System;
using UnityEngine.UI;
using Rewired;
[Serializable]
public enum DriveType
{
	RearWheelDrive,
	FrontWheelDrive,
	AllWheelDrive
}

public class WheelDrive : MonoBehaviour
{
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
    public float currentAngle = 0f;
    public float steerSmooth;
    private float baseSteerSmooth = 0.1f;
    private float steerVelocity = 0.0F;
    [Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
    public float handbrakeTorque = 50000f;
   
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;
    public Vector3 centerOfMass;
    public int airFloat;
    public int pushSpeed;
    public int yawSpeed;
    public int rotSpeed;
    public int pitchSpeed;
    public AnimationCurve[] torqueCurves;
    public bool isReverse;
    public int[] topSpeed;
    public float currentSpeed;
    [Range(0, 5)]
    public int gear;
    public Text gearDisplay;
    public Text kphDisplay;
    public Text debugDisplay;
	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
    public float currentTorque;
    public float accelTest;
    public bool Redlined = false;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

    public RectTransform tachNeedle;
    public float idleTachAngle;
    public float redlineTachAngle;
	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;
    public Rigidbody rb;
    private WheelCollider[] m_Wheels;
    public Player controller;
    // Find all the WheelColliders down in the hierarchy.
	void Start()
    {
        controller = ReInput.players.GetPlayer(0);
        Mathf.Clamp(gear, 0, 5);
        baseSteerSmooth = steerSmooth;
        gear = 1;
        rb.centerOfMass = centerOfMass;
        m_Wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}

    // This is a really simple approach to updating wheels.
    // We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
    // This helps us to figure our which wheels are front ones and which are rear.
    void Update()
    {
        if (controller.GetButtonDown("GearUp") && gear < topSpeed.Length - 1)
            gear++;
        
        else if (controller.GetButtonDown("GearDown") && gear > 0)
            gear--;

        if (gear == 0)
            gearDisplay.text = "R";
        else
        gearDisplay.text = gear + " / " + (topSpeed.Length - 1);

        tachNeedle.rotation = Quaternion.Euler(180,0,  accelTest * 200f);
       
    }
    void FixedUpdate()
	{
        if (controller.GetButton("SlowMo"))
            Time.timeScale = 0.5f;
        else
            Time.timeScale = 1f;

        currentSpeed = rb.velocity.magnitude * 3.6f;
        
        accelTest = (currentSpeed / topSpeed[gear]);

       
        kphDisplay.text = Mathf.RoundToInt(currentSpeed) + " k/h";
        currentTorque = (torqueCurves[gear].Evaluate(currentSpeed / topSpeed[gear]));

        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);


        if (currentSpeed <= topSpeed[gear])
        {
            Redlined = false;
            rb.AddRelativeForce(Vector3.forward * controller.GetAxis("Accelerate") * pushSpeed * gear * currentTorque);
        }
        else
        {
            Redlined = true;
        }

        float inputAngle = maxAngle * controller.GetAxisRaw("Steering");
        steerSmooth = baseSteerSmooth + (currentSpeed * 0.0008f);
        currentAngle = Mathf.SmoothDamp(currentAngle, inputAngle, ref steerVelocity, steerSmooth);
        float torque = controller.GetAxis("Accelerate") * (maxTorque * currentTorque);
       

        float brake = 0;
        //in-air control forces
        if (controller.GetButton("Handbrake"))
        {
            brake = handbrakeTorque;
        }
        else
        {
            brake = controller.GetAxis("Brake") * brakeTorque;
        }

        //float handBrake = Input.GetKey(KeyCode.Space) ? brakeTorque : 0;

        

        foreach (WheelCollider wheel in m_Wheels)
		{

            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = currentAngle;
                wheel.brakeTorque = controller.GetAxis("Brake") * brakeTorque;
            }
			if (wheel.transform.localPosition.z < 0)
			{
                if (!controller.GetButton("Handbrake"))
                {
                    wheel.brakeTorque = controller.GetAxis("Brake") * brakeTorque;
                }
                else
                {
                    wheel.brakeTorque = handbrakeTorque;
                }
            }

            if (Redlined == false)
            {
                if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
                {
                    wheel.motorTorque = torque;
                }

                if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
                {
                    wheel.motorTorque = torque;
                }
                debugDisplay.text = "" + wheel.motorTorque;
            }
            else
            {
                wheel.brakeTorque = brakeTorque;
                wheel.motorTorque = 0;
            }

			// Update visual wheels if any.
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);
				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
        Vector3 down = transform.TransformDirection(Vector3.down);
        if (!Physics.Raycast(transform.position, down, 0.25f))
        {
            rb.AddForce(Vector3.up * airFloat * (60 * Time.fixedDeltaTime));
            rb.AddRelativeTorque(Vector3.up * (currentAngle / maxAngle) * (yawSpeed / 3));
            rb.AddRelativeTorque(Vector3.back * controller.GetAxis("Roll") * rotSpeed);
            rb.AddRelativeTorque(Vector3.right * controller.GetAxis("Pitch") * pitchSpeed);
        }
        else //ground control forces
        {
            rb.AddRelativeTorque(Vector3.up * controller.GetAxis("Steering") * yawSpeed);
        }
        //handbrake yaw power change
        if (controller.GetButton("Handbrake"))
            rb.AddRelativeTorque(Vector3.up * controller.GetAxis("Steering") * yawSpeed * 2);
    }
}
