using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController[] playerControllers;
    // Start is called before the first frame update
    void Start()
    {
        playerControllers = FindObjectsOfType<PlayerController>();
    }

}
