using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System;

public class NetworkScript : MonoBehaviour
{

    static SocketIOComponent SocketIO;
    public GameObject character1;
    public GameObject character2;

    //public GameObject mainChracter;
    public GameObject[] OtherPlayers= new GameObject[20];
    // Use this for initialization
    private int otherPlayersCount = 0;
    void Start()
    {
        SocketIO = GetComponent<SocketIOComponent>();
        SocketIO.On("identify", OnIdentify);
        SocketIO.On("anotherplayerconnected", OtherPlayer);
        SocketIO.On("move", OnMove);
    }

    private void OnMove(SocketIOEvent obj)
    {
        Debug.Log("Another player is moving "+obj.data);
    }

    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

    }
  
    void OnIdentify(SocketIOEvent Obj)
    {
        //TO DO CREATE DYNAMICALLY AN OBJECT
        //mainChracter = Instantiate(character1, GetVectorPositonFromJson(Obj.data), Quaternion.identity);
         var players = Obj.data.GetField("allPlayersAtCurrentTime");
          for (int i = 0; i < players.list.Count; i++)
          {
              string playerKey = (string)players.keys[i];
              JSONObject playerData = (JSONObject)players.list[i];
              // Process the player key and data as you need.
              GameObject Player = Instantiate(character2, GetVectorPositonFromJson(playerData), Quaternion.identity);
              OtherPlayers[otherPlayersCount++] = Player;
          }
        
    }

    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }


    Vector3 GetVectorPositonFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString()), float.Parse(Json["y"].ToString()), float.Parse(Json["z"].ToString()));
    }

    void OtherPlayer(SocketIOEvent Obj)
    {
        //mainChracter = Instantiate(character2, GetVectorPositonFromJson(Obj.data), Quaternion.identity);
        GameObject Player = Instantiate(character2, GetVectorPositonFromJson(Obj.data), Quaternion.identity);
        OtherPlayers[otherPlayersCount++] = Player;

    }

}