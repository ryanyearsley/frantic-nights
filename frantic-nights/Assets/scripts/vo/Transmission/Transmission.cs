using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Transmission : ScriptableObject
{
    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    public AnimationCurve[] torqueCurves;
    public float[] topSpeeds;
    public float shiftTime = 0.1f;
}
