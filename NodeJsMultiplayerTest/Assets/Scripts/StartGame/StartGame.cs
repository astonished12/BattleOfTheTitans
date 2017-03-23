using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public NetworkRegisterLogin connectionMaker;
    //TO DO SEND LOGIN DATA TO SERVER
    public void LoginOnClick()
    {
        connectionMaker.SendLoginData();
    }
    public void ChangeLevel () {
        SceneManager.LoadScene(1);
    }
	
	
}
