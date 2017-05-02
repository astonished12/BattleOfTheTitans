using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepPlayer : MonoBehaviour {

    public Target final;
    public float lastAttackScan;
    public float scanRate;
    private float minDistance = 10;

    void Start()
    {
        final = GetComponent<Target>();
    }
    void Update()
    {
        if (isReadyToScan())
        {
            FindClosestEnemy();
            lastAttackScan = Time.time;

        }
    }

    // Use this for initialization
    private bool isReadyToScan()
    {
        return (Time.time - lastAttackScan > scanRate && gameObject.tag != "EnemyMinions");
    }

    private void FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMinions");

        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {            
                float curDistance = GetDistanceBetweenPositions(enemy.transform.position, position);
                if (curDistance < minDistance)
                {
                    if (GetComponent<CreepAi>().isMovingOn == false && enemy.GetComponent<CreepAi>().isMovingOn == false)
                    {
                        GetComponent<CreepAi>().isMovingOn = true;
                        enemy.GetComponent<CreepAi>().isMovingOn = true;
                        //TO DO SEND TO SERVER AND FOLLOW THE REMOTE FROM THE OTHER CLIENT
                        GetComponent<NetworkCommunication>().SendMinionDataToFollow(GetComponent<NetworkEntity>().Id, enemy.GetComponent<NetworkEntity>().Id);
                    }
                }            
        }
    }

    private float GetDistanceBetweenPositions(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow(end.y - start.y, 2));
    }
}
