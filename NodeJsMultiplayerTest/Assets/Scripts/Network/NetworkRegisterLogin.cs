using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class NetworkRegisterLogin : MonoBehaviour
{

	SocketIOComponent SocketIO;
    public static List<string> RoomList = new List<string>();
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
                RoomList.Add(room_id);
            }
        }
    } 
    public void OnNewRoom(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        RoomList.Add(socket_id);
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
