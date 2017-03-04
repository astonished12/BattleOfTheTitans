using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offSet;

    void Start()
    {
        offSet = transform.position - player.transform.position;
    }

     void LateUpdate()
    {
        transform.position = player.transform.position + offSet;
    }
}
