using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{


    public PlayerInputs currentInput;

    private VehicleController vehicleController;
    public Player player;

    private Rigidbody rb;
    private Vector3 startingLocation;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        vehicleController = GetComponent<VehicleController>();
        rb = GetComponent<Rigidbody>();
        startingLocation = transform.position;
        startingRotation = transform.rotation;

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
    }

    // Update is called once per frame
    void Update()
    {
        vehicleController.updateVehicle(currentInput);
        if (player.GetButtonDown("Reset"))
        {
            print("player reset");
            resetPlayer();
        }

        if (player.GetButtonDown("ToggleControlsText"))
        {
            print("player controller toggle ctrls");
            GameManager.instance.toggleControlsDisplay();
        }


    }

    public void resetPlayer()
    {
        vehicleController.resetVehicle(startingLocation, startingRotation);
    }
}
