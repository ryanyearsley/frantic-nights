using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePhysicsUtility : MonoBehaviour
{
  public static float wheelRpmToKph(float wheelRpm, float wheelDiameter)
    {
        return ((wheelRpm * 60) * wheelDiameter) / 1000;
    }
}
