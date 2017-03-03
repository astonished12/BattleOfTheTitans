using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour {

    public GameObject player;
    private List<Node> path;
    int targetIndex;


    public void OnClickPressed(Vector3 targetPosition)
    {
        Debug.Log(targetPosition);
        var resetGrid = GetComponent<Grid>();
        resetGrid.ResetGrid();
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(player.transform.position, targetPosition);
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0].worldPosition;
        while (true)
        {
            if (player.transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count-1)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint, 20 * Time.deltaTime);
            yield return null;

        }
    }

}
