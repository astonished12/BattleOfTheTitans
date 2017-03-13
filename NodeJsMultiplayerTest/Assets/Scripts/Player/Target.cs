using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public Transform targetTransform;

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
