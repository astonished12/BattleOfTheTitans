using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public NetworkRegisterLogin connectionMaker;
    public void ChangeLevel () {
        SceneManager.LoadScene(1);
        connectionMaker.SendPlay();
    }
	
	
}
