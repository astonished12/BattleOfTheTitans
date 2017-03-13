﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

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
        networkEntity = GetComponent<NetworkEntity>();
    }

    void Update()
    {
        if (isReadyToAttack() && target.IsInRange(attackDistance))            
        {
            lastAttackTime = Time.time;
            var networkEntityIdOfTarget = target.GetComponent<NetworkEntity>().Id;
             networkCommunication.SendAttackerId(networkEntityIdOfTarget);
        }
    }

    private bool isReadyToAttack()
    {
        return (Time.time - lastAttackTime > attackRate && target.targetTransform);
    }
}