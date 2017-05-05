using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour, IClickable {

    private GameObject player;
    public GameObject listOfCharacter;
    private CursorController myCourseController;

    private void Awake()
    {
        player = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
        myCourseController = GetComponent<CursorController>();

    }
    public void OnClick(RaycastHit hit)
    {
        myCourseController.ChangeToMoveCursor();

        if (player.GetComponent<Alive>().isAlive)
        {
            
            var navigator = player.GetComponent<NavagiateToPosition>();
            navigator.SetTargetPosition(hit.point);

            var networkCommunication = player.GetComponent<NetworkCommunication>();
            networkCommunication.SendLastPositionToNodeServer(hit.point);
        }
    }

  

}
