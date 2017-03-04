using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour {

    public GameObject player;
    private List<Node> path;
    
    public void OnClickPressed(Vector3 targetPosition)
    {
        
        var navigator = player.GetComponent<NavagiateToPosition>();
        navigator.SetTargetPosition(targetPosition);

        var networkCommunication = player.GetComponent<NetworkCommunication>();
        networkCommunication.SendLastPositionToNodeServer(targetPosition);       
    }    

}
