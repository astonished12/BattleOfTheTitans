using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : MonoBehaviour {

    public float health = 100f;
    Animator animator;
    float respawnTime = 5f;
    public bool isAlive { get { return health > 0; } }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnHit()
    {
        health -= 10;
        if (!isAlive)
        {
            animator.SetTrigger("dead");
            Invoke("Respawn", respawnTime);
        }


    }

    void Respawn()
    {
        transform.position = new Vector3(-47f, 0f, 16.5f);
        health = 100;
        animator.SetTrigger("Respawn");
    }
}
