using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    public GameObject listOfCharacter;

    private Vector3 offSet;
    private float distance  = 60;
    private float sensitivityDistance  = 100;
    private float damping  = 5;
    private float minFOV  = 30;
    private float maxFOV  = 80;
    void Start()
    {
        offSet = new Vector3(0,15,7);
        distance = GetComponent<Camera>().fieldOfView;
        player = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
    }

    //zoom camera
 
    void Update()
    {

        distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, distance, Time.deltaTime * damping);
    }


    void LateUpdate()
    {
        transform.position = player.transform.position + offSet;
    }
}
