using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    Target target;
    public float attackDistance;
    public float lastAttackTime;
    public float attackRate;

    void Start()
    {
        target = GetComponent<Target>();
    }

    void Update()
    {
        if (isReadyToAttack() && target.IsInRange(attackDistance))            
        {
            lastAttackTime = Time.time;
            Debug.Log("ATTACK");
        }
    }

    private bool isReadyToAttack()
    {
        return (Time.time - lastAttackTime > attackRate && target.targetTransform);
    }
}
