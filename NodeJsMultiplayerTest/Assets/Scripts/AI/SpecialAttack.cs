using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public float damage;
    public bool inAction;
    public Texture2D pictureSkill;
    public GameObject fireBall;
    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(key) && key.ToString()=="Q")
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
            GameObject g = Instantiate(fireBall, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
            /*float forceDirX, forceDirY, forceDirZ;
            forceDirX = Random.Range(-10, 10);
            forceDirY = Random.Range(5, 15);
            forceDirZ = Random.Range(-10, 10);*/

            Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = (Input.mousePosition - sp).normalized;
            g.GetComponent<Rigidbody>().AddForce(dir * 5);

            //g.GetComponent<Rigidbody>().AddForce(new Vector2(wp.x, wp.y), 0);

            //g.GetComponent<Rigidbody>().AddForce(forceDirX * 100, forceDirY * 100, forceDirZ * 100);
            Destroy(g, 5);
        }
	}
}
