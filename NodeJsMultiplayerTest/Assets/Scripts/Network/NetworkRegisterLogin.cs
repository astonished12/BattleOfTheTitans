﻿using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class NetworkRegisterLogin : MonoBehaviour
{

	SocketIOComponent SocketIO;
    public static Dictionary<string,Room> RoomList = new Dictionary<string,Room>();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SocketIO = GetComponent<SocketIOComponent>();
        SocketIO.On("allRooms", OnEnter);
        SocketIO.On("newRoom", OnNewRoom);
        SocketIO.On("closeRoom", OnCloseRoom);
        SocketIO.On("joinSuccesFull", OnJoinSucces);
    }

    public void SendLoginData()
    {
       //SocketIO.Emit("login");
    }

    public void OnEnter(SocketIOEvent Obj)
    {
        var rooms = Obj.data.GetField("allRoomsAtCurrentTime");
        var socket_id = ElementFromJsonToString(Obj.data.GetField("socket_id").ToString())[1];
      
        for (int i = 0; i < rooms.list.Count; i++)
        {
            string room_id = (string)rooms.keys[i];
            if (room_id != socket_id)
            {
                JSONObject roomData = (JSONObject)rooms.list[i];
                Room aux = new Room(int.Parse(roomData["maxPlayers"].ToString().Replace("\"", "")), int.Parse(roomData["maxPlayers"].ToString().Replace("\"", "")), roomData["name"].ToString().Replace("\"", ""), room_id);             
                RoomList.Add(room_id,aux);
            }
        }
    } 
    public void OnNewRoom(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        Room aux = new Room(int.Parse(obj.data["currentPlayers"].ToString().Replace("\"", "")), int.Parse(obj.data["maxPlayers"].ToString().Replace("\"", "")), obj.data["name"].ToString().Replace("\"", ""), socket_id);
        RoomList.Add(socket_id, aux);
        Debug.Log("new room here ");
    }

    public void OnCloseRoom(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        RoomList.Remove(socket_id);
        Debug.Log("Room closed ");
    }

    private void OnJoinSucces(SocketIOEvent obj)
    {
        Debug.Log("JOIN SUCCES");
        //string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
      /*  string room_id = ElementFromJsonToString(obj.data["room_id"].ToString())[1];
        Debug.Log(room_id);     
       
        RoomList[room_id].currentNumberOfPlayers++;
        // AICI E PROBLEMA CU room_id TO DOOOOOoo*/
        //ROOM SAU NU E BAGAT IN VECTOR NU TREBUIE CRED
        SceneManager.LoadScene(2);
        SocketIO.Emit("play");

    }




    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }
}
