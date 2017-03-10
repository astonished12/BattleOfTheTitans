using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour, IClickable {

    public GameObject player;
    private List<Node> path;
    
    public void OnClick(RaycastHit hit)
    {
        
        var navigator = player.GetComponent<NavagiateToPosition>();
        navigator.SetTargetPosition(hit.point);

        var networkCommunication = player.GetComponent<NetworkCommunication>();
        networkCommunication.SendLastPositionToNodeServer(hit.point);       
    }    

}
