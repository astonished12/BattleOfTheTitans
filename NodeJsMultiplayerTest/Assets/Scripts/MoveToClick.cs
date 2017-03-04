using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour {

    public GameObject player;
    private List<Node> path;
    
    public void OnClickPressed(Vector3 targetPosition)
    {
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(player.transform.position, targetPosition);
        var navigator = player.GetComponent<NavagiateToPosition>();
        navigator.SetDestination(path);

        var networkCommunication = player.GetComponent<NetworkCommunication>();
        networkCommunication.SendLastPositionToNodeServer(path[path.Count-1].worldPosition);       
    }    

}
