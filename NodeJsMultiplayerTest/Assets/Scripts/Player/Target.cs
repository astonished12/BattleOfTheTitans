using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public string targetName;
    public Transform targetTransform;
    public Vector3 pos;
    public float offset = 0.5f;
    public void SetTargetTransform(Transform _transform)
    {
        targetTransform = _transform;
    }

    public bool IsInRange(float stopFollowDistance)
    {
        var currentDistance = Vector3.Distance(transform.position, targetTransform.position);
        return currentDistance < stopFollowDistance;
    }

}
