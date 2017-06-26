using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HistoryMatch : MonoBehaviour {

    private SocketIOComponent SocketIO;
    private Dictionary<int, GameObject> matchList = new Dictionary<int, GameObject>();
    public GameObject matchHistoryPrefab;
    public GameObject contentParent;
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
        matchList = new Dictionary<int, GameObject>();
        JSONObject matches = obj.data.GetField("matches");
        Debug.Log(matches);
        for (int i = 0; i < matches.Count; i++)
        {
            string playerName = myJsonParser.ElementFromJsonToString(matches[i].GetField("namePlayer").ToString())[1];
            string oponentName = myJsonParser.ElementFromJsonToString(matches[i].GetField("nameOponent").ToString())[1];
            string owner = matches[i].GetField("owner").ToString().Replace("\"", "");
            string status = matches[i].GetField("status").ToString().Replace("\"", "");
            string totalDmg = matches[i].GetField("totalDamage").ToString().Replace("\"","");

            GameObject newMatch = Instantiate(matchHistoryPrefab);
            newMatch.transform.FindChild("Text").GetComponent<Text>().text = "Match " + i+" (click to view stats)";
            newMatch.transform.SetParent(contentParent.transform, false);
            newMatch.GetComponent<Match>().playerName = playerName;
            newMatch.GetComponent<Match>().oponentName = oponentName;
            newMatch.GetComponent<Match>().owner = owner;
            newMatch.GetComponent<Match>().status = status;
            newMatch.GetComponent<Match>().totalDamage = totalDmg;

            matchList.Add(i, newMatch);
           
           
        }
    }

    private void DestroyAllMatchesGameObjects()
    {
        foreach (int key in matchList.Keys)
        {
            Destroy(matchList[key]);
        }
    }
}
