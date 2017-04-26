﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System;

public class NetworkScript : MonoBehaviour
{

    public static SocketIOComponent SocketIO;
    public SpawnerPlayer spawner;
    private GameObject player;
    public GameObject listOfChracter;

    private  bool ownerFlag;
    private GameObject[] objects;
    //public GameObject mainChracter;

    private void Awake()
    {
        player = listOfChracter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;   
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
    }
    // Use this for initialization
    void Start()
    {        
        SocketIO.On("identify", OnIdentify);
        //SocketIO.On("anotherplayerconnected", OtherPlayer);
        SocketIO.On("playerLeft", OnPlayerLeft);
        SocketIO.On("playerMove", OnMove);
        SocketIO.On("followPlayer", OnFollow);
        SocketIO.On("followTower", OnFollowTower);
        SocketIO.On("attackPlayer", OnAttack);
    }

    

    void OnIdentify(SocketIOEvent Obj)
    {
        player.transform.position = GetVectorPositionFromJson(Obj.data);
        ownerFlag = Convert.ToBoolean(ElementFromJsonToString(Obj.data.GetField("owner").ToString())[1]);
        //Debug.Log(" Hostul are owner " + Obj.data["owner"] +ownerFlag);
        var players = Obj.data.GetField("allPlayersAtCurrentTime");
        var idTowers = Obj.data.GetField("towersId");
        Debug.Log(idTowers);
        var socket_id = ElementFromJsonToString(Obj.data.GetField("socket_id").ToString())[1];
        player.GetComponent<NetworkEntity>().Id = socket_id;
        player.GetComponent<NetworkEntity>().ownerFlag = ownerFlag;

        //IDENTIFY TOWERS      
        objects = GameObject.FindGameObjectsWithTag("clickable");       
        var objectCount = objects.Length;
        int k = 0;  
        foreach (var obj in objects)
        {            
            obj.GetComponent<FollowTower>().myPlayer = player;
            obj.GetComponent<NetworkEntity>().Id = idTowers[k].ToString();
            k++;        
        }    
          
        spawner.AddMyPlayer(socket_id, player);

        for (int i = 0; i < players.list.Count; i++)
        {
            string playerKey = (string)players.keys[i];
            if (playerKey != socket_id)
             {
                JSONObject playerData = (JSONObject)players.list[i];
                int noRemoteCharacters = int.Parse(ElementFromJsonToString(playerData.GetField("characterNumber").ToString())[1]);
                bool ownerFlagAux = Convert.ToBoolean(ElementFromJsonToString(playerData.GetField("isOwner").ToString())[1]);
                //Debug.Log(" la remote avem owner " + playerData["isOwner"]+ " "+ ownerFlagAux);
                spawner.SpawnPlayer(playerKey, noRemoteCharacters,GetVectorPositionFromJson(playerData), ownerFlagAux);
            }
        }
        
    }

    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }
 
    Vector3 GetVectorPositionFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString().Replace("\"","")), float.Parse(Json["y"].ToString().Replace("\"","")), float.Parse(Json["z"].ToString().Replace("\"", "")));
    }

   

    private void OnPlayerLeft(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data.GetField("socket_id").ToString())[1];
        spawner.PlayerLeft(socket_id);       
    }

    private void OnMove(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];      
        Vector3 targetPostion = GetVectorPositionFromJson(obj.data);
        var otherPlayer = spawner.OtherPlayersGameObjects[socket_id];
        var walker = otherPlayer.GetComponent<NavagiateToPosition>();
        walker.SetTargetPosition(targetPostion);
    }

    private void OnFollow(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];

        //remote
        var playerWhoDoRequest = spawner.OtherPlayersGameObjects[socket_id];
        //client player
        var target = spawner.OtherPlayersGameObjects[target_id];        

        var followerOfPlaeryRequested = playerWhoDoRequest.GetComponent<Target>();
        followerOfPlaeryRequested.SetTargetTransform(target.transform);
    }
    private void OnFollowTower(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];

        Debug.Log("CINE PE CINE "+socket_id + " " + target_id);
    }
    public void OnAttack(SocketIOEvent obj)
    {
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];

        var targetOfAttacker = spawner.OtherPlayersGameObjects[target_id];
        var attacker = spawner.OtherPlayersGameObjects[socket_id];


        targetOfAttacker.transform.rotation = Quaternion.LookRotation(attacker.transform.position - targetOfAttacker.transform.position);
        attacker.transform.rotation = Quaternion.LookRotation(targetOfAttacker.transform.position - attacker.transform.position);

        attacker.GetComponent<Animator>().SetFloat("multiplier", 2);
        attacker.GetComponent<Animator>().SetTrigger("attack");

        spawner.SpawnBullet(socket_id,attacker.transform.position, targetOfAttacker.transform);
    }

}

