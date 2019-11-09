using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    //Physics
    public Rigidbody rb;
    public Vector3 centerOfMass;
    public int airFloat;
    public int pushSpeed;
    public int yawSpeed;
    public int rotSpeed;
    public int pitchSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = centerOfMass;
    }

    public void fixedUpdatePhysics(PlayerInputs playerInputs)
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        if (!Physics.Raycast(transform.position, down, 0.25f))
        {
            rb.AddForce(Vector3.up * airFloat * (60 * Time.fixedDeltaTime));
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * (yawSpeed / 3));
            rb.AddRelativeTorque(Vector3.back * playerInputs.rollInput * rotSpeed);
            rb.AddRelativeTorque(Vector3.right * playerInputs.pitchInput * pitchSpeed);
        }
        else //ground control forces
        {
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed);
        }
        //handbrake yaw power change
        if (playerInputs.handBrakeButton)
            rb.AddRelativeTorque(Vector3.up * playerInputs.steeringInput * yawSpeed * 2);

        rb.AddRelativeForce(Vector3.forward * playerInputs.accelInput * pushSpeed);

    }
}
