using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRegisterLogin : MonoBehaviour
{

	 SocketIOComponent SocketIO;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SocketIO = GetComponent<SocketIOComponent>();
    }

    public void SendPlay()
    {
        SocketIO.Emit("play");
    }
}
