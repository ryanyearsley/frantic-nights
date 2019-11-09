using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public PlayerInputs currentInput;

    private VehicleController vehicleController;
    private PhysicsController physicsController;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        vehicleController = GetComponent<VehicleController>();
        physicsController = GetComponent<PhysicsController>();
    }

    private void FixedUpdate()
    {
        generatePlayerInputs();
        vehicleController.fixedUpdateVehicle(currentInput);
    }

    private void generatePlayerInputs()
    {
        currentInput.accelInput = player.GetAxisRaw("Accelerate");
        currentInput.brakeInput = player.GetAxisRaw("Brake");
        currentInput.clutchInput = 1f;
        currentInput.steeringInput = player.GetAxisRaw("Steering");
        currentInput.gearUpButtonDown = player.GetButtonDown("GearUp");
        currentInput.gearDownButtonDown = player.GetButtonDown("GearDown");
        currentInput.handBrakeButton = player.GetButton("Handbrake");

        currentInput.pitchInput = player.GetAxis("Pitch");
        currentInput.rollInput = player.GetAxis("Roll");
        currentInput.yawInput = player.GetAxis("Steering");

        vehicleController.fixedUpdateVehicle(currentInput);
        physicsController.fixedUpdatePhysics(currentInput);
    }

    // Update is called once per frame
    void Update()
    {
        vehicleController.updateVehicle();
    }
}
