﻿using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardHandler : MonoBehaviour {

    public static SocketIOComponent SocketIO;
    private KeyCode[] keys;
    Ray myRay;
    RaycastHit hit = new RaycastHit();
    float timePressedQ;

    JSONParser myJsonParser = new JSONParser();
    private void Awake()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        keys = new KeyCode[5];
        keys[0] = KeyCode.Q;
        keys[1] = KeyCode.W;
        keys[2]= KeyCode.E;
        keys[3] = KeyCode.R;
    }

    // Update is called once per frame
    void Update() {
        //To get the current mouse position
        //Convert the mousePosition according to World position
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(keys[0]))
        {
            timePressedQ = Time.time;
        }
        if (Time.time-timePressedQ<0.25 && ActionBar.skillSlots[0].coolDownActive == false && Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(myRay, out hit))
            {                
                SocketIO.Emit("keyPressed", new JSONObject(myJsonParser.QKeyToJson(keys[0].ToString(),hit.point)));
            }
                
        }
        if (Input.GetKeyDown(keys[1]) && ActionBar.skillSlots[1].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(myJsonParser.KeyIdToJson(keys[1].ToString())));
        }
        if (Input.GetKeyDown(keys[2]) && ActionBar.skillSlots[2].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(myJsonParser.KeyIdToJson(keys[2].ToString())));
        }
        if (Input.GetKeyDown(keys[3]) && ActionBar.skillSlots[3].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(myJsonParser.KeyIdToJson(keys[3].ToString())));
        }

    }

   

}
