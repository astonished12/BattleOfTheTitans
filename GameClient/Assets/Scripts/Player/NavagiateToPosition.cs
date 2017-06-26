using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavagiateToPosition : MonoBehaviour
{

    Target target;
    Animator animator;

    public int targetIndex;
    public List<Node> path;
    public Vector3 currentTargetPosition;
    public float speed = 1f;

    public bool targetSuccesfull = true;

    public void Awake()
    {
        target = GetComponent<Target>();
        animator = GetComponent<Animator>();
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        currentTargetPosition = _targetPosition;
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(transform.position, currentTargetPosition);        
        SetDestination(path);
        target.SetTargetTransform(null);
        animator.SetBool("attack", false);

    }

    public void SetTargetMinion(Vector3 _targetPosition)
    {
        currentTargetPosition = _targetPosition;
        var pathfinder = GetComponent<PathFinder>();
        path = pathfinder.AStar(transform.position, currentTargetPosition);
        SetDestination(path);
        target.SetTargetTransform(null);
      }   
    public void SetDestination(List<Node> _path){
        targetSuccesfull = false;       
        path = _path;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }
    IEnumerator FollowPath()
    {
        Vector3[] currentWaypoints = SimplifyPath(path);
        if (targetIndex > 0 || targetIndex < currentWaypoints.Length)
        {
            GetComponent<Animator>().SetBool("atDestination", false);
            Vector3 currentWaypoint = currentWaypoints[targetIndex];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Count - 1)
                    {
                        targetSuccesfull = true;
                        targetIndex = 0;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex].worldPosition;

                }
                Vector3 rotateVector = currentWaypoint - transform.position;
                if (rotateVector != Vector3.zero)
                {
                    var rotation = Quaternion.LookRotation(currentWaypoint - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    void Update()
    {
        if (targetSuccesfull && !GetComponent<CreepAi>())
        {
            GetComponent<Animator>().SetBool("atDestination", true);
        }
        if (targetSuccesfull && GetComponent<CreepAi>() && GetComponent<CreepAi>().posibleTarget)
        {
            if (gameObject)
            {
                GetComponent<Animator>().SetBool("atDestination", true);
                transform.rotation = Quaternion.LookRotation(GetComponent<CreepAi>().posibleTarget.transform.position - transform.position);
            }
        }
    }
    public void SetFinalTarget()
    {
        if (targetSuccesfull)
        {
            GetComponent<Target>().targetTransform = GetComponent<CreepAi>().final;
        }  
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridPosition.x - path[i].gridPosition.x, path[i - 1].gridPosition.y - path[i].gridPosition.y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

}
