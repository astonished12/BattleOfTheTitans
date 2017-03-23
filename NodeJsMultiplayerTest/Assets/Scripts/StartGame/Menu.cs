using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button exitButton;
    public SocketIOComponent SocketIO;

    public GameObject menu;
    public GameObject joinMenu;
    public GameObject createMenu;
    //private Button playButton;
    // Use this for initialization
    void Start () {

        joinMenu.SetActive(false);
        createMenu.SetActive(false);
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();

        createRoomButton.onClick.AddListener(() => {
           SocketIO.Emit("newRoom");
           
       });
        /*playButton.onClick.AddListener(() => {
            SocketIO.Emit("play");
            SceneManager.LoadScene(2);
        });*/
        exitButton.onClick.AddListener(() => { Application.Quit(); });

    }

    public void SendPlay()
    {
        SocketIO.Emit("play");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
