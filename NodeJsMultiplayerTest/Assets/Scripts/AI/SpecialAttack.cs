using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public int damage;
    public bool inAction;
    public Texture2D pictureSkill;
    public GameObject objectToSpawn;
    float distance = 10f;
    public Vector3 targetPositions;
    

    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update()
    {
        if (key.ToString() == "Q" && inAction) {
            Debug.Log("Special attack ON " + key.ToString());
          
            GameObject go = Instantiate(objectToSpawn, targetPositions, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 4);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[0].coolDownActive = true;
                ActionBar.skillSlots[0].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;
            //}
        }

        if (key.ToString() == "W" && inAction )
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 2);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[1].coolDownActive = true;
                ActionBar.skillSlots[1].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }

        if (key.ToString() == "E" && inAction)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn,transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 5);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[2].coolDownActive = true;
                ActionBar.skillSlots[2].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }

        if (key.ToString() == "R" && inAction)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 4);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[3].coolDownActive = true;
                ActionBar.skillSlots[3].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }


    }

}
