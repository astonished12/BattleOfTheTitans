using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkRegisterLogin : MonoBehaviour
{

	 SocketIOComponent SocketIO;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SocketIO = GetComponent<SocketIOComponent>();
        SocketIO.On("newRoom", OnNewRoom);
    }     

    public void SendLoginData()
    {
       //SocketIO.Emit("login");
    }

    public void OnNewRoom(SocketIOEvent obj)
    {
        Debug.Log("new room here");
    }
   
}
