using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepPlayer : MonoBehaviour {

    public Target final;
    public float lastAttackScan;
    public float scanRate;
    private float minDistance = 2;

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
                    if ((enemy.GetComponent<CreepAi>() && GetComponent<FollowerMinion>().mustStop == 0 && enemy.GetComponent<FollowerMinion>().mustStop == 0) 
                      || ((GetComponent<CreepAi>() && GetComponent<FollowerMinion>().mustStop == 0 && enemy.GetComponent<CreepAi>()==null)))
                        {
                         GetComponent<FollowerMinion>().mustStop = 1;
                             if(enemy.GetComponent<CreepAi>())
                                enemy.GetComponent<FollowerMinion>().mustStop = 1;
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
