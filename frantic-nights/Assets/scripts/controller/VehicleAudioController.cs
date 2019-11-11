using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAudioController : MonoBehaviour
{
    public AudioSource engineSourceHigh;
    private AudioSource exhaustAudioSource;

    public AudioClip engineClipHigh;

    private float maxEngineRpm;

    private List<AudioSource> tireAudioSources = new List<AudioSource>();


    public void updateVehicleAudio(float engineRpm, float accelInput)
    {
        float highRPM = 0f;
        highRPM = Mathf.Lerp(-1f, 1f, engineRpm / maxEngineRpm);

        highRPM = Mathf.Clamp01(highRPM) * 1;

        float volumeLevel = Mathf.Clamp(accelInput, 0.1f, 1f);
        float pitchLevel = Mathf.Lerp(engineSourceHigh.pitch, Mathf.Lerp(0.1f, 1f, engineRpm / 7000f), Time.fixedDeltaTime * 50f);

        engineSourceHigh.volume = volumeLevel;
        engineSourceHigh.pitch = pitchLevel;

        engineSourceHigh.pitch = pitchLevel;

                if (!engineSourceHigh.isPlaying)
            engineSourceHigh.Play();

        }
    }
