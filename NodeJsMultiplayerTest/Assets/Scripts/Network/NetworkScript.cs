using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System;

public class NetworkScript : MonoBehaviour
{

    public static SocketIOComponent SocketIO;
    public SpawnerPlayer spawner;
    public GameObject player;
    //public GameObject mainChracter;
  
    private void Awake()
    {
        
        SocketIO = GetComponent<SocketIOComponent>();
        Debug.Log("In network script " + SocketIO.GetInstanceID());
    }
    // Use this for initialization
    void Start()
    {
        
        SocketIO.On("identify", OnIdentify);
        SocketIO.On("anotherplayerconnected", OtherPlayer);
        SocketIO.On("playerLeft", OnPlayerLeft);
        SocketIO.On("playerMove", OnMove);
    }

   

    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

    }
  
    void OnIdentify(SocketIOEvent Obj)
    {
        Debug.Log("IDENTIFY");
        player.transform.position = GetVectorFromJson(Obj.data);
        var players = Obj.data.GetField("allPlayersAtCurrentTime");
        var socket_id = ElementFromJsonToString(Obj.data.GetField("socket_id").ToString())[1];
        for (int i = 0; i < players.list.Count; i++)
        {

            string playerKey = (string)players.keys[i];
            if (playerKey != socket_id)
             {
                JSONObject playerData = (JSONObject)players.list[i];
                spawner.SpawnPlayer(playerKey, GetVectorFromJson(playerData));
            }
        }
        
    }

    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }


 
    Vector3 GetVectorFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString().Replace("\"","")), float.Parse(Json["y"].ToString().Replace("\"","")), float.Parse(Json["z"].ToString().Replace("\"", "")));
    }
    void OtherPlayer(SocketIOEvent Obj)
    {
        Debug.Log("OTHER PLAYER");
        string socket_id = ElementFromJsonToString(Obj.data.GetField("socket_id").ToString())[1];
        spawner.SpawnPlayer(socket_id, GetVectorFromJson(Obj.data));
    }
    private void OnPlayerLeft(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data.GetField("socket_id").ToString())[1];
        spawner.PlayerLeft(socket_id);
       
    }

    private void OnMove(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];      
        Vector3 targetPostion = GetVectorFromJson(obj.data);
        var otherPlayer = spawner.OtherPlayersGameObjects[socket_id];
        var walker = otherPlayer.GetComponent<NavagiateToPosition>();
        walker.SetTargetPosition(targetPostion);
    }
}

