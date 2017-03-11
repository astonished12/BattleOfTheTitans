using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    public Transform target;
    NavagiateToPosition navigator;
    public float scanFrequnecy = 0.5f;
    public float stopFollowDistance = 3;
    float lastScanTime = 0;
    private void Start()
    {
        navigator = GetComponent<NavagiateToPosition>();
        
    }

    private void Update()
    {
        if(isReadyToScan() && !isInRange())
        {
            navigator.SetTargetPosition(target.position-new Vector3(0.5f,0.5f,0.5f));            
        }
    }

    private bool isInRange()
    {
        var currentDistance = Vector3.Distance(transform.position, target.position- new Vector3(0.5f, 0.5f, 0.5f));        
        return currentDistance < stopFollowDistance;
    }

    private bool isReadyToScan()
    {
        return (Time.time - lastScanTime > scanFrequnecy && target);
    }
}
