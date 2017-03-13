using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToClick : MonoBehaviour,IClickable{

    public Follower myPlayerFollower;
    private NetworkEntity networkEntity;

    private void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
    }
    public void OnClick(RaycastHit hit)
    {
        myPlayerFollower.SetTarget(transform);

        var networkCommunication = GetComponent<NetworkCommunication>();
        networkCommunication.SendPlayerIdToFollow(networkEntity.Id);
    }

   
}
