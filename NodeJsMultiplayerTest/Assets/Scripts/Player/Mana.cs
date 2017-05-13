using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {

    public float maxMana;
    public float curMana;
    public Image manaBar;
    float lastTimeUpdateMana;
    float periodToUpdate = 10f;
    void Awake()
    {
        curMana = maxMana;
        UpdateHealthBar();
    }

    void Update()
    {
        if (Time.time - lastTimeUpdateMana > periodToUpdate)
        {
            OnManaUp(3);
        }
    }
    void UpdateHealthBar()
    {
        float ratio = curMana / maxMana;
        manaBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void OnManaUp(int damage)
    {
        if (GetComponent<Alive>().isAlive)
        {
            if (curMana + damage < maxMana)
            {
                curMana += damage;
                UpdateHealthBar();
            }
            else
            {
                curMana = maxMana;
                UpdateHealthBar();
            }
        }
        lastTimeUpdateMana = Time.time;
    }

    public void DecreaseMana(int damage)
    {
        OnManaUp(-damage);
    }

    public void Respawn()
    {
        curMana = maxMana;

        manaBar.rectTransform.localScale = new Vector3(1, 1, 1);
    }
}
