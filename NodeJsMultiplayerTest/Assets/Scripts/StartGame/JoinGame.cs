﻿using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour {

    public Text roomText;
    public SocketIOComponent SocketIO;
    void Start()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
    }
    public void SetupRoom(string matchName, int currentNumberOfPlayers,int maxNumberOfPlayers)
    {
        roomText.text = matchName + "(" + currentNumberOfPlayers + "/" + maxNumberOfPlayers + ")";
    }

    public void JoinOnRoom()
    {
        string id = GetComponent<NetworkEntity>().Id;
        SocketIO.Emit("joinRoom", new JSONObject(RoomIdToJson(id)));
    }

      private string RoomIdToJson(string id)
    {
        return string.Format(@"{{""idRoom"":""{0}""}}", id);
    }
}
