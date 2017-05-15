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

    //CHANGE TO MENU IF DATA IS OK
    public void ChangeLevel () {
        SceneManager.LoadScene(2);
    }
	
    //BACK BUTTON
    public void ChangeLevelToRegister()
    {
        SceneManager.LoadScene(1);
    }

}
