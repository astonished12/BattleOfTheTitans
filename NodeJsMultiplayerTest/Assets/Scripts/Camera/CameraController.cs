using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    public GameObject listOfCharacter;

    private Vector3 offSet;

    void Start()
    {
        offSet = new Vector3(0,15,7);
        player = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
    }



     void LateUpdate()
    {
        transform.position = player.transform.position + offSet;
    }
}
