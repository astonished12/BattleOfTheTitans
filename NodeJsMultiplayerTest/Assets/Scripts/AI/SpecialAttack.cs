using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public int damage;
    public bool inAction;
    public Texture2D pictureSkill;
    public GameObject objectToSpawn;
    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update()
    {
        if (key.ToString() == "Q" && inAction) {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn,transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 4);
            inAction = false;
        }

        if (key.ToString() == "W" && inAction)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 2);
            inAction = false;

        }

        if (key.ToString() == "E" && inAction)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn,transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 5);
            inAction = false;

        }

        if (key.ToString() == "R" && inAction)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            Destroy(go, 4);
            inAction = false;

        }


    }

}
