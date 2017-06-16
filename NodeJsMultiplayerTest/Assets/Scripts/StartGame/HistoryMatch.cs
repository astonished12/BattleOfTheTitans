using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HistoryMatch : MonoBehaviour {

    private SocketIOComponent SocketIO;
    private Dictionary<string, GameObject> matchList = new Dictionary<string, GameObject>();
    JSONParser myJsonParser = new JSONParser();

    void Start()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.Emit("getMyMatches", new JSONObject(myJsonParser.MessageToJson(NetworkRegisterLogin.UserName)));
        SocketIO.On("listMatches", OnReceiveListMatches);

    }

    private void OnReceiveListMatches(SocketIOEvent obj)
    {
        DestroyAllMatchesGameObjects();
        matchList = new Dictionary<string, GameObject>();
        JSONObject matches = obj.data.GetField("matches");
        Debug.Log(matches);
    }

    private void DestroyAllMatchesGameObjects()
    {
        foreach (string key in matchList.Keys)
        {
            Destroy(matchList[key]);
        }
    }
}
