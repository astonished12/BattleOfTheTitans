using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardHandler : MonoBehaviour {

    public static SocketIOComponent SocketIO;
    private KeyCode[] keys;
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
        if (Input.GetKeyDown(keys[0]) && ActionBar.skillSlots[0].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(KeyIdToJson(keys[0].ToString())));
        }
        if (Input.GetKeyDown(keys[1]) && ActionBar.skillSlots[1].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(KeyIdToJson(keys[1].ToString())));
        }
        if (Input.GetKeyDown(keys[2]) && ActionBar.skillSlots[2].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(KeyIdToJson(keys[2].ToString())));
        }
        if (Input.GetKeyDown(keys[3]) && ActionBar.skillSlots[3].coolDownActive == false)
        {
            SocketIO.Emit("keyPressed", new JSONObject(KeyIdToJson(keys[3].ToString())));
        }

    }

    private string KeyIdToJson(string id)
    {
        return string.Format(@"{{""key"":""{0}""}}", id);
    }
}
