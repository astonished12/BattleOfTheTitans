using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAi : MonoBehaviour
{
    public static int numberOrder;
    Vector3 offset;
    public Target final;
    public float lastAttackScan;
    public float scanRate;

    private float minDistance = 100;
    void Start()
    {
        final = GetComponent<Target>();
    }
    public Vector3 ComputeOffset()
    {
        if (CreepAi.numberOrder == 1)
        {
            offset = new Vector3(-3f, 0f, 0f);
        }
        if (CreepAi.numberOrder == 2)
        {
            offset = new Vector3(-3f, 0f, -5f);
        }

        if (CreepAi.numberOrder == 3)
        {
            offset = new Vector3(-3f, 0f, 5f);
        }
        return offset;
    }

    public void Update()
    {
        if (isReadyToScan())
        {
            FindClosestEnemy();
            lastAttackScan = Time.time;

        }
    }

    private bool isReadyToScan()
    {
        return (Time.time - lastAttackScan > scanRate && gameObject.tag!="EnemyMinions");
    }

    private void FindClosestEnemy()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMinions");
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (gameObject != enemy)
            {
                var diff = (enemy.transform.position - position);
                var curDistance = diff.sqrMagnitude;
                if (curDistance < minDistance)
                {
                    Debug.Log("ar trebui sa ma opresc mininul");
                    GetComponent<Follower>().target.SetTargetTransform(null);
                    GetComponent<Follower>().navigator.SetTargetBase(enemy.transform.position);
                }
                else
                {
                    Debug.Log(curDistance);
                   //Debug.Log("Already found something to target.");
                }
            }
        }
    }
}
 

