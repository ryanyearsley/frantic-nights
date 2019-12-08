using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{

    public GameObject playerIndicator;

    //private RawImage miniMap;
    //private Camera miniMapCamera;
    private Transform cameraTrans;
    private Transform playerTrans;
    private Transform playerIndicatorTrans;



    // Start is called before the first frame update
    void Start()
    {
        cameraTrans = this.transform;
        playerTrans = FindObjectOfType<PlayerController>().transform;
        playerIndicatorTrans = Instantiate(playerIndicator).transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cameraTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y + 20, playerTrans.position.z);


        playerIndicatorTrans.SetPositionAndRotation(playerTrans.position, playerTrans.rotation);
    }
}
