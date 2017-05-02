using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public float damage;
    public bool inAction;
    public Texture2D pictureSkill;
    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(key))
        {
            inAction = true;
            Debug.Log("Special attack ON " + key.ToString());
        }
	}
}
