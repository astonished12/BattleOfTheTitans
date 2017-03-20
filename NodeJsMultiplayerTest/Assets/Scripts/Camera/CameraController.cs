using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offSet;

    void Start()
    {
        offSet = new Vector3(0,15,7);        
    }


     void LateUpdate()
    {
        transform.position = player.transform.position + offSet;
    }
}
