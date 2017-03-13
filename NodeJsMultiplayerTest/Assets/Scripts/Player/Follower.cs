using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    public Target target;

    NavagiateToPosition navigator;
    public float scanFrequnecy = 0.1f;
    public float stopFollowDistance = 1;
    float lastScanTime = 0;
    private void Start()
    {
        navigator = GetComponent<NavagiateToPosition>();
        target = GetComponent<Target>();       
    }
 
    private void Update()
    {
        
        if (isReadyToScan() && !target.IsInRange(stopFollowDistance)) 
        {
            navigator.SetTargetFollowPosition(target.targetTransform.position);
        }
    }


    private bool isReadyToScan()
    {
        return (Time.time - lastScanTime > scanFrequnecy && target.targetTransform);
    }
}
