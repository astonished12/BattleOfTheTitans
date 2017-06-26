using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToClick : MonoBehaviour,IClickable{

    public GameObject myPlayer;
    private NetworkEntity networkEntity;

    private Target myPlayerTarget;
    private CursorController myCourseController;

    private void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
        myPlayerTarget = myPlayer.GetComponent<Target>();
        myCourseController = GetComponent<CursorController>();

    }
    public void OnClick(RaycastHit hit)
    {
        myCourseController.ChangeToMoveCursor();

        myPlayerTarget.targetTransform = gameObject.transform;
        myPlayerTarget.targetName = gameObject.name;
        

        var networkCommunication = GetComponent<NetworkCommunication>();
        networkCommunication.SendPlayerIdToFollow(networkEntity.Id);
    }

  
}
