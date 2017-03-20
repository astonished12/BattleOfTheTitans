using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alive : MonoBehaviour {
      
    public float maxHealth;
    public float curHealth;
    Animator animator;
    float respawnTime = 5f;

    public Image healthBar;

    public bool isAlive { get { return curHealth > 0; } }
    void Awake()
    {
        animator = GetComponent<Animator>();
        curHealth = maxHealth;
        UpdateHealthBar();
    }
    public void OnHit(int damage)
    {
        curHealth -= damage;
        UpdateHealthBar();
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

    void UpdateHealthBar()
    {
        float ratio = curHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
