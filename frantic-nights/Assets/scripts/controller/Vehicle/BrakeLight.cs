using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLight : MonoBehaviour
{
    public MeshRenderer lightMesh;
    public Light pointLight;
    public TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lightMesh = transform.Find("lightMesh").GetComponent<MeshRenderer>();
        pointLight = transform.Find("light").GetComponent<Light>();
        trailRenderer = lightMesh.gameObject.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
