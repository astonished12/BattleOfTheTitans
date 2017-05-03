using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour {

    public int damage;
    public float cast=5;
    public bool isActive=true;

    void Update()
    {
        if (isActive == false)
        {
            StartCoroutine(CastSkill());
        }
    }

    IEnumerator CastSkill()
    {
        yield return new WaitForSeconds(cast);
        isActive = true;
    }
}
