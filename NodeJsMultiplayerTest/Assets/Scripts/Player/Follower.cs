using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    private Transform target;
    NavagiateToPosition navigator;
    public float scanFrequnecy = 0.1f;
    public float stopFollowDistance = 1;
    float lastScanTime = 0;
    private void Start()
    {
        navigator = GetComponent<NavagiateToPosition>();
        
    }

    public void SetTarget(Transform _target)
    {
        target = _target;

        Debug.Log(transform.position + " " + target.position);

    }
    private void Update()
    {
        //Debug.Log(transform.position + " " + target.position);
        if(isReadyToScan() && !isInRange())
        {
            navigator.SetTargetPosition(target.position);            
        }
    }

    private bool isInRange()
    {
        var currentDistance = Vector3.Distance(transform.position, target.position);        
        return currentDistance < stopFollowDistance;
    }

    private bool isReadyToScan()
    {
        return (Time.time - lastScanTime > scanFrequnecy && target);
    }
}
