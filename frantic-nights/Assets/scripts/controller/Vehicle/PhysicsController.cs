using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    //Physics
    [SerializeField]
    private bool isGrounded;
    public Transform trans;
    public Rigidbody rb;
    public Vector3 centerOfMass;
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
        rb.centerOfMass = centerOfMass;
    }

    public void fixedUpdatePhysics(PlayerInputs playerInputs, VehicleWheelMessage vwm)
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        isGrounded = Physics.Raycast(transform.position, down, 0.25f);
        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * airFloat * playerInputs.accelInput);
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed);
            rb.AddRelativeTorque(Vector3.back * playerInputs.rollInput * rotSpeed);
            rb.AddRelativeTorque(Vector3.right * playerInputs.pitchInput * pitchSpeed);
        }
        else //ground control forces
        {


            rb.AddRelativeTorque(Vector3.back * playerInputs.rollInput * groundRotSpeed);
            rb.AddRelativeTorque(Vector3.right * playerInputs.pitchInput * groundPitchSpeed);

            float yawSpeedGoverner = (100f - (rb.velocity.magnitude * 3.6f))/100;
            if (yawSpeedGoverner < 0)
                yawSpeedGoverner = 0;

            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed * yawSpeedGoverner);
            rb.AddRelativeForce(Vector3.down * downforce);
        }
        //handbrake yaw power change
        if (playerInputs.handBrakeButton)
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed * 2);

        if (!vwm.isRedlined)
        rb.AddRelativeForce(Vector3.forward * (playerInputs.accelInput + -playerInputs.brakeInput) * pushSpeed);

    }
}
