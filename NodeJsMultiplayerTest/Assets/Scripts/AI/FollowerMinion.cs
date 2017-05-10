using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMinion : MonoBehaviour {

	public Target target;
    public NavagiateToPosition navigator;
    public float scanFrequnecy = 0.1f;
    public float stopFollowDistance=3f;
    float lastScanTime = 0;
    private Vector3 offSet;
    private void Start()
    {
        navigator = GetComponent<NavagiateToPosition>();
        target = GetComponent<Target>();       
    }
 
    private void Update()
    {
        if (isReadyToFollowMinion())
        {
            AddOffset();
            navigator.SetTargetMinion(target.targetTransform.position + offSet + GetComponent<CreepAi>().ComputeOffset(GetComponent<CreepAi>().number));
        }

        if (isReadyToScan() && !target.IsInRange(stopFollowDistance))
        {
            AddOffset();            
            navigator.SetTargetMinion(target.targetTransform.position - offSet);            
        }    
    }

    void AddOffset()
    {
        if (transform.position.x > target.targetTransform.position.x)
            offSet = Vector3.left * stopFollowDistance * 0.75f;
        else if (transform.position.x < target.targetTransform.position.x)
            offSet = Vector3.right * stopFollowDistance * 0.75f;

    }

    private bool isReadyToFollowMinion()
    {
        return (Time.time - lastScanTime > scanFrequnecy && target.targetTransform && GetComponent<CreepAi>().posibleTarget);
    }

    private bool isReadyToScan()
    {
        return (Time.time - lastScanTime > scanFrequnecy && target.targetTransform);
    }

   
}
