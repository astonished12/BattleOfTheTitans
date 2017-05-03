using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public float damage;
    public bool inAction;
    public Texture2D pictureSkill;
    public GameObject objectToSpawn;
    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && key.ToString() == "Q")
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
            //GameObject g = Instantiate(objectToSpawn, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                
                var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                position = Camera.main.ScreenToWorldPoint(position);
                GameObject go = Instantiate(objectToSpawn, position, Quaternion.identity) as GameObject;
                Destroy(go, 4);

            }

        }

        if (Input.GetKeyDown(key) && key.ToString() == "W")
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
            GameObject g = Instantiate(objectToSpawn, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
            Destroy(g, 5);
        }

        if (Input.GetKeyDown(key) && key.ToString() == "E")
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
            GameObject g = Instantiate(objectToSpawn, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
            Destroy(g, 5);
        }

        if (Input.GetKeyDown(key) && key.ToString() == "R")
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
            Destroy(go, 4);
            

           

           
        }
    }
}
