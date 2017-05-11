using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepPlayer : MonoBehaviour {

    public Target final;
    public float lastAttackScan;
    public float scanRate;
    private float minDistance = 3;
    public bool hasEnemy = false;

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
        hasEnemy = false;

        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {            
                float curDistance = GetDistanceBetweenPositions(enemy.transform.position, position);
                if (curDistance < minDistance)
                {
                hasEnemy = true;
                if ((enemy.GetComponent<CreepAi>() && GetComponent<FollowerMinion>().mustStop == false && enemy.GetComponent<FollowerMinion>().mustStop == false) || ((GetComponent<CreepAi>() && GetComponent<FollowerMinion>().mustStop == false && enemy.GetComponent<CreepAi>()==null)))
                    {
                        GetComponent<FollowerMinion>().mustStop = true;
                         if(enemy.GetComponent<CreepAi>())
                            enemy.GetComponent<FollowerMinion>().mustStop = true;
                        //TO DO SEND TO SERVER AND FOLLOW THE REMOTE FROM THE OTHER CLIENT
                        GetComponent<NetworkCommunication>().SendMinionDataToFollow(GetComponent<NetworkEntity>().Id, enemy.GetComponent<NetworkEntity>().Id);
                        break;
                    }
                }            
        }
     

    }

    private float GetDistanceBetweenPositions(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow(end.y - start.y, 2));
    }
}
