using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputController : MonoBehaviour
{

    public Player controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
