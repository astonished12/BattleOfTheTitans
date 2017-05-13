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
    private float lastTimeUpdateHealth;
    private float periodToUpdate = 5f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        curHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (Time.time - lastTimeUpdateHealth > periodToUpdate)
        {
            OnHealUp(3);
        }
    }
    public void OnHit(GameObject owner,int damage)
    {
        curHealth -= damage;
        UpdateHealthBar();
        if (!isAlive && !GetComponent<FollowTower>() && !GetComponent<CreepAi>() && !GetComponent<NetworkEntity>().isTower)
        {
            animator.SetTrigger("dead");
            //CHECK IS ANOTHER PLAYER WHO KILL 
            if (owner && !owner.GetComponent<NetworkEntity>().isTower)
              owner.GetComponent<Target>().targetTransform = null;
            Invoke("Respawn", respawnTime);
           
        }
        else if(!isAlive && GetComponent<NetworkEntity>().isTower)
        {
            Invoke("DestroyTower", 1.0f);
        }
        else if(!isAlive && GetComponent<CreepAi>())
        {
            Invoke("DestroyMinion", 1.0f);
        }
    }

    public void OnHealUp(int damage)
    {
        if (isAlive && !GetComponent<FollowTower>())
        {
            if (curHealth + damage < maxHealth)
            {
                curHealth += damage;
                UpdateHealthBar();
            }
            else
            {
                curHealth = maxHealth;
                UpdateHealthBar();
            }
            lastTimeUpdateHealth = Time.time;

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

        if (GetComponent<Mana>())
        {

            GetComponent<Mana>().Respawn();
        }

        animator.SetTrigger("Respawn");
    }
    private void DestroyTower()
    {
        Destroy(gameObject);
    }
    private void DestroyMinion()
    {
        GetComponent<Animator>().SetTrigger("dead");
        Destroy(gameObject,2f);
    }
    void UpdateHealthBar()
    {
        float ratio = curHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
