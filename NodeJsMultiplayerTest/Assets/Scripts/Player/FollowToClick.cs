using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToClick : MonoBehaviour,IClickable{

    public GameObject myPlayer;
    private NetworkEntity networkEntity;

    Target myPlayerTarget;

    private void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
        myPlayerTarget = myPlayer.GetComponent<Target>();
    }
    public void OnClick(RaycastHit hit)
    {

        myPlayerTarget.targetTransform = transform;

        var networkCommunication = GetComponent<NetworkCommunication>();
        networkCommunication.SendPlayerIdToFollow(networkEntity.Id);
    }

   
}
