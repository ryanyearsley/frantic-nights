using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            other.transform.GetComponentInParent<PlayerController>().resetPlayer();
        }
    }
}
