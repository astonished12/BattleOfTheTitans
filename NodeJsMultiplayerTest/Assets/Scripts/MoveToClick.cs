using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour {

    public GameObject player;

    public void OnClickPressed(Vector3 targetPosition)
    {

        var pathfinder = GetComponent<PathFinder>();
        pathfinder.AStar(player.transform.position, targetPosition);
    }

}
