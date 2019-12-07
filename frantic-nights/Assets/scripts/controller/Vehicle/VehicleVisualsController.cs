using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleVisualsController : MonoBehaviour
{
    public BrakeLight[] brakeLights;
    public Material brakeLightOffMaterial;
    public Material brakeLightOnIdle;
    public Material brakeLightOnEngaged;

    private float brakeLightIdleIntensity = 0.2f;
    private float brakeLightEngagedIntensity = 1f;

    private bool brakeLightsEngaged;

    private static float brakeDeadzoneThreshold = 0.02f;
    public void initializeVisualsController()
    {
        brakeLights = GetComponentsInChildren<BrakeLight>();
    }


    public void updateVehicleVisuals(PlayerInputs pi)
    {
        updateBrakeLights(pi);
    }

    private void updateBrakeLights(PlayerInputs pi)
    {
        if (pi.brakeInput > brakeDeadzoneThreshold && !brakeLightsEngaged)
        {
            engageBrakeLight();
        }

        if (pi.brakeInput < brakeDeadzoneThreshold && brakeLightsEngaged)
        {
            disengageBrakeLight();
        }
    }


    public void engageBrakeLight()
    {
        foreach (BrakeLight brakeLight in brakeLights)
        {
            brakeLight.lightMesh.material = brakeLightOnEngaged;
            brakeLight.pointLight.intensity = brakeLightEngagedIntensity;
            brakeLight.trailRenderer.emitting = true;
        }
        brakeLightsEngaged = true;
    }

    public void disengageBrakeLight()
    {
        foreach (BrakeLight brakeLight in brakeLights)
        {
            brakeLight.lightMesh.material = brakeLightOnIdle;
            brakeLight.pointLight.intensity = brakeLightIdleIntensity;
            brakeLight.trailRenderer.emitting = false;
        }
        brakeLightsEngaged = false;

    }

}
