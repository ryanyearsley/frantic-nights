using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    //Physics
    [SerializeField]
    private bool isGrounded;
    private bool airMode;
    public Transform trans;
    public Rigidbody rb;
    public Vector3 groundedCenterOfMass;
    public Vector3 inAirCenterOfMass;
    public int airFloat;
    public int downforce;
    public int pushSpeed;
    public int yawSpeed;
    public int rotSpeed;
    public int pitchSpeed;
    public int groundYawSpeed;
    public int groundRotSpeed;
    public int groundPitchSpeed;
    // Start is called before the first frame update
    void Start()
    {
        trans = transform.parent;
        rb = trans.GetComponent<Rigidbody>();
        rb.centerOfMass = groundedCenterOfMass;
    }

    public void fixedUpdatePhysics(PlayerInputs playerInputs, VehicleWheelMessage vwm)
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        isGrounded = Physics.Raycast(transform.position, down, 0.25f);
        if (!isGrounded)
        {
            //in-air
            rb.AddRelativeForce(Vector3.forward * pushSpeed * playerInputs.accelInput);
            rb.AddForce(Vector3.down * pushSpeed * playerInputs.brakeInput);

            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed);
            rb.AddRelativeTorque(Vector3.back * playerInputs.rollInput * rotSpeed);
            rb.AddRelativeTorque(Vector3.right * playerInputs.pitchInput * pitchSpeed);
            if (rb.centerOfMass.y != inAirCenterOfMass.y)
                rb.centerOfMass = inAirCenterOfMass;
        }
        else 
        {
            //ground control forces
            rb.AddRelativeTorque(Vector3.back * playerInputs.rollInput * groundRotSpeed);
            rb.AddRelativeTorque(Vector3.right * playerInputs.pitchInput * groundPitchSpeed);

            float yawSpeedGoverner = (100f - (rb.velocity.magnitude * 3.6f))/100;
            if (yawSpeedGoverner < 0)
                yawSpeedGoverner = 0;

            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed * yawSpeedGoverner);
            rb.AddRelativeForce(Vector3.down * downforce);

            if (rb.centerOfMass.y != groundedCenterOfMass.y)
                rb.centerOfMass = groundedCenterOfMass;
        }
        //handbrake yaw power change
        if (playerInputs.handBrakeButton)
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed * 2);

        if (!vwm.isRedlined)
        rb.AddRelativeForce(Vector3.forward * (playerInputs.accelInput + -playerInputs.brakeInput) * pushSpeed);

    }
}
