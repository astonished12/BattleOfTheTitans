using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JoinGame : MonoBehaviour {

    public Text roomText;
    public SocketIOComponent SocketIO;
    public string id;
    void Start()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
    }
   

    public void SetupRoom(string matchName, int currentNumberOfPlayers,int maxNumberOfPlayers,string _id)
    {
        id = _id;
        roomText.text = matchName + "(" + currentNumberOfPlayers + "/" + maxNumberOfPlayers + ")";
    }

    public void JoinOnRoom()
    {   
        SocketIO.Emit("joinRoom", new JSONObject(RoomIdToJson(id)));
    }

      private string RoomIdToJson(string id)
      {
            return string.Format(@"{{""idRoom"":""{0}""}}", id);
      }
}
