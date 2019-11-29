using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Vehicle : ScriptableObject
{
    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    public AnimationCurve[] torqueCurves;
    public float[] topSpeeds;
    public GearTest[] gearTests;
    public float shiftTime = 0.1f;

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


}
