﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    Target target;
    public float attackDistance;
    public float lastAttackTime;
    public float attackRate;
    NetworkEntity networkEntity;
    NetworkCommunication networkCommunication;


    void Start()
    {
        target = GetComponent<Target>();
        networkCommunication = GetComponent<NetworkCommunication>();
    }

    void Update()
    {


        if (!isReadyToAttack())
            return;
       
        if (target.targetTransform.GetComponent<Alive>() && !target.targetTransform.GetComponent<Alive>().isAlive)
        {
            target.SetTargetTransform(null);
            return;
        }

       
        if (isReadyToAttack() && target.IsInRange(attackDistance) && target.targetTransform.GetComponent<Alive>() && target.targetTransform.GetComponent<Alive>().isAlive && !GetComponent<CreepAi>())
        {
            var networkEntityIdOfTarget = target.targetTransform.GetComponent<NetworkEntity>().Id;
            networkCommunication.SendAttackerId(networkEntityIdOfTarget);
            lastAttackTime = Time.time;

        }

    }

    private bool isReadyToAttack()
    {
        return (Time.time - lastAttackTime > attackRate && target.targetTransform);
    }

    
}
