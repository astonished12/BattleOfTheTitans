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
    public void OnHit(GameObject owner,int damage)
    {
        curHealth -= damage;
        UpdateHealthBar();
        if (!isAlive && !GetComponent<FollowTower>())
        {
            animator.SetTrigger("dead");
            owner.GetComponent<Target>().targetTransform = null;
            Invoke("Respawn", respawnTime);
           
        }
        else if(!isAlive && GetComponent<FollowTower>())
        {
            Invoke("DestroyTower", 1.0f);
        }
    }

    void Respawn()
    {
        Debug.Log("A murit ownerul camerei ?!" + GetComponent<NetworkEntity>().ownerFlag);
        //To DO SPAWNER POSITION FOR EACH PLAYER + NO INPUTS
        if (GetComponent<NetworkEntity>().ownerFlag)
        {
            transform.position = GameObject.Find("Base1").transform.position;
            Debug.Log("Ma duc in baza rosie "+ GameObject.Find("Base1").transform.position);
        }
        else
        {
            transform.position = GameObject.Find("Base2").transform.position;
            Debug.Log("Ma duc in baza blue "+ GameObject.Find("Base2").transform.position);

        }

        curHealth = maxHealth;
               
        healthBar.rectTransform.localScale = new Vector3(1, 1, 1);

        animator.SetTrigger("Respawn");
    }
    private void DestroyTower()
    {
        Destroy(gameObject);
    }
    void UpdateHealthBar()
    {
        float ratio = curHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
