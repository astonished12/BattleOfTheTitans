using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavagiateToPosition : MonoBehaviour
{

    Target target;


    int targetIndex;
    private List<Node> path;
    private Vector3 targetPositon;
    public float speed = 1f;

    public bool targetSuccesfull = true;

    public void Awake()
    {
        target = GetComponent<Target>();
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPositon = _targetPosition;
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(transform.position, targetPositon);        
        SetDestination(path);
        target.SetTargetTransform(null);
    }

    public void SetTargetFollowPosition(Vector3 _targetPosition)
    {
        targetPositon = _targetPosition;
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(transform.position, targetPositon);
       //path.RemoveAt(path.Count - 1);
        SetDestination(path);

    }
    public void SetDestination(List<Node> _path)
    {
        targetSuccesfull = false;
        GetComponent<Animator>().SetBool("atDestination", false);
        path = _path;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0].worldPosition;
        currentWaypoint.y = 0.5f;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count - 1)
                {
                    targetSuccesfull = true;
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
                currentWaypoint.y = 0.5f;

            }
            transform.rotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime );
            yield return null;

        }
    }

    void Update()
    {
        if (targetSuccesfull)
            GetComponent<Animator>().SetBool("atDestination", true);
    }
    
    
}
