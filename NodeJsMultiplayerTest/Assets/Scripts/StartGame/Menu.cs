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
    public Button refreshRoomList;
    public SocketIOComponent SocketIO;
    
    public Text joinStatus;


    public GameObject menu;
    public GameObject joinMenu;
    public GameObject createMenu;

    public GameObject itemListPrefab;
    public GameObject roomListParent;

    public Button closeRoom;
    //private Button playButton;
    // Use this for initialization
    void Start () {

        joinMenu.SetActive(false);
        createMenu.SetActive(false);
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();

        closeRoom.onClick.AddListener(() => {
            joinMenu.SetActive(false);
            createMenu.SetActive(false);
            menu.SetActive(true);
            SocketIO.Emit("closeRoom");           
       });

        createRoomButton.onClick.AddListener(() => {
            menu.SetActive(false);
            joinMenu.SetActive(false);
            createMenu.SetActive(true);
            SocketIO.Emit("newRoom");
        });

        joinRoomButton.onClick.AddListener(() =>
        {
            menu.SetActive(false);
            createMenu.SetActive(false);
            joinMenu.SetActive(true);
            if (NetworkRegisterLogin.RoomList.Count==0)
            {
                joinStatus.text = "There is no room here";
            }
            else
            {
                foreach(string room in NetworkRegisterLogin.RoomList)
                {
                    GameObject roomListItemGo = Instantiate(itemListPrefab);
                    roomListItemGo.transform.SetParent(roomListParent.transform);
                }
            }
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
