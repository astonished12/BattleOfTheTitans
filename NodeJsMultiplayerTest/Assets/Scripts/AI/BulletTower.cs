using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : MonoBehaviour
{

    public Transform target;
    public GameObject tower;
    public float speed;
    TowerAtack BT;
    // Use this for initialization
    void Start()
    {
        //StartCoroutine("Die");
        BT = tower.gameObject.GetComponent<TowerAtack>();
       
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position+new Vector3(0f,1f,0f), step);
    }


    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyMinions")
        {
            Destroy(gameObject);
        }

    }
}