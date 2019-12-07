using System;
using UnityEngine;
using System.Collections;

public class DriftCamera : MonoBehaviour
{
    public enum CameraView
    {
        ThirdPerson, Hood, Side
    }
    [Serializable]
    public class AdvancedOptions
    {
        public bool updateCameraInUpdate;
        public bool updateCameraInFixedUpdate = true;
        public bool updateCameraInLateUpdate;
        public KeyCode switchViewKey = KeyCode.X;
        public KeyCode thirdPersonKey = KeyCode.Alpha1;
        public KeyCode firstPersonKey = KeyCode.Alpha2;

    }

    public CameraView currentCameraView;
    public float smoothing = 6f;
    public Transform lookAtTarget;
    public Transform positionTarget;
    public Transform sideView;
    public Transform hoodView;
    public AdvancedOptions advancedOptions;

    bool m_ShowingSideView;

    public bool cinematicOpening;
    public Transform cinematicStartingLocation;
    public Transform cinematicTargetLocation;
    public float cinematicCameraSpeed = 0.1f;

    private void Start()
    {
        if (cinematicOpening)
        {
            transform.position = cinematicStartingLocation.position;
            //StartCoroutine(cinematicCameraOpening());
        }
    }

    private IEnumerator cinematicCameraOpening ()
    {
        yield return new WaitForSeconds(1);
        while (transform.position != cinematicStartingLocation.position)
            transform.position = Vector3.Lerp(transform.position, cinematicStartingLocation.position, Time.deltaTime * cinematicCameraSpeed);
    }

    private void FixedUpdate ()
    {
        if(advancedOptions.updateCameraInFixedUpdate)
            UpdateCamera ();
    }

    private void Update ()
    {
        if (Input.GetKeyDown(advancedOptions.thirdPersonKey))
        {
            transform.parent = null;
            currentCameraView = CameraView.ThirdPerson;
        }
        if (Input.GetKeyDown(advancedOptions.firstPersonKey))
        {
            currentCameraView = CameraView.Hood;
            transform.parent = hoodView;
            transform.position = hoodView.position;
            transform.rotation = hoodView.rotation;
        }
        if (Input.GetKeyDown (advancedOptions.switchViewKey))
            currentCameraView = CameraView.Side;

        if (advancedOptions.updateCameraInUpdate)
            UpdateCamera ();
    }

    private void LateUpdate ()
    {
        if(advancedOptions.updateCameraInLateUpdate)
            UpdateCamera ();
    }

    private void UpdateCamera ()
    {
        switch (currentCameraView)
        {
            case CameraView.ThirdPerson:
                transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
                transform.LookAt(lookAtTarget);

                break;
            case CameraView.Side:
                transform.position = sideView.position;
                transform.rotation = sideView.rotation;
                break;
        }

    }
}
