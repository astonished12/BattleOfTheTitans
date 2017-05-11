using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMinion : MonoBehaviour
{

    // put the points from unity interface
    public Transform[] wayPointList = new Transform[6];

    public int currentWayPoint = 0;
    Transform targetWayPoint;

    public float speed = 4f;

    public bool mustStop=false;
    // Use this for initialization
    public void SetWayPoints(GameObject minionsWaypoints)
    {
        for(int i = 0; i < 6; i++)
        {
            wayPointList[0] = minionsWaypoints.transform.GetChild(i).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
                targetWayPoint = wayPointList[currentWayPoint];
            if(mustStop==false)
                Walk();
        }
    }

    void Walk()
    {
        if (gameObject && targetWayPoint && transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            if (currentWayPoint < wayPointList.Length)
                targetWayPoint = wayPointList[currentWayPoint];
            else
                mustStop = true;
        }
        if (targetWayPoint)
        // rotate towards the target
        {
            transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

            // move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);
        }
      
    }
}
