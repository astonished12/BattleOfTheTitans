using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : MonoBehaviour {
      
    public float maxHealth;
    public float curHealth;
    Animator animator;
    float respawnTime = 5f;
    public bool isAlive { get { return curHealth > 0; } }
    void Awake()
    {
        animator = GetComponent<Animator>();
        curHealth = maxHealth;
    }
    public void OnHit(int damage)
    {
        curHealth -= damage;
        GetComponent<Bars>().AddjustCurrentHealth(damage);
        if (!isAlive)
        {
            animator.SetTrigger("dead");
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        //To DO SPAWNER POSITION FOR EACH PLAYER + NO INPUTS 
        transform.position = new Vector3(-47f, 0f, 16.5f);
        curHealth = 100;
        animator.SetTrigger("Respawn");
    }
}
