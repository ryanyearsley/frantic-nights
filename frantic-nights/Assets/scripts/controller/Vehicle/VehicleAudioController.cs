using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAudioController : MonoBehaviour
{

    private OptionsData optionsData;



    public AudioSource engineSourceHigh;
    private AudioSource exhaustAudioSource;

    public AudioClip engineClipHigh;

    private float maxEngineRpm = 8000f;

    private List<AudioSource> tireAudioSources = new List<AudioSource>();

    public void initializeAudio()
    {
        optionsData = SaveGameManager.loadOptions();
    }
    
    
    public void updateVehicleAudio(float engineRpm, float accelInput)
    {
        float highRPM = 0f;
        highRPM = Mathf.Lerp(-1f, 1f, engineRpm / maxEngineRpm);

        highRPM = Mathf.Clamp01(highRPM) * 1;

        float volumeLevel = Mathf.Clamp(accelInput, 0.6f, 1f);
        float pitchLevel = Mathf.Lerp(engineSourceHigh.pitch, Mathf.Lerp(0.1f, 1f, engineRpm / 7000f), Time.fixedDeltaTime * 50f);

        engineSourceHigh.volume = optionsData.sfxVolume;
        engineSourceHigh.pitch = pitchLevel;

        engineSourceHigh.pitch = pitchLevel;

                if (!engineSourceHigh.isPlaying)
            engineSourceHigh.Play();

        }
    }
