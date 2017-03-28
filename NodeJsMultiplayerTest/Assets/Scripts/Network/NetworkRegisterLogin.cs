using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

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
                Room aux = new Room(int.Parse(roomData["maxPlayers"].ToString().Replace("\"", "")), int.Parse(roomData["maxPlayers"].ToString().Replace("\"", "")), roomData["name"].ToString().Replace("\"", ""));             
                RoomList.Add(room_id,aux);
            }
        }
    } 
    public void OnNewRoom(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        Room aux = new Room(int.Parse(obj.data["currentPlayers"].ToString().Replace("\"", "")), int.Parse(obj.data["maxPlayers"].ToString().Replace("\"", "")), obj.data["name"].ToString().Replace("\"", ""));
        RoomList.Add(socket_id, aux);
        Debug.Log("new room here ");
    }

    public void OnCloseRoom(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        RoomList.Remove(socket_id);
        Debug.Log("Room closed ");
    }


    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }
}
