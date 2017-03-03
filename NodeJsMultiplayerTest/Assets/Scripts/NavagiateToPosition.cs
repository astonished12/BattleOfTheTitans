using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavagiateToPosition : MonoBehaviour
{
    int targetIndex;
    private List<Node> path;
    public float speed = 20f;

    public void SetDestination(List<Node> _path)
    {
        path = _path;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0].worldPosition;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count - 1)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    
}
