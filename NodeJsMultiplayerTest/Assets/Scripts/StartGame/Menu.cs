using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button exitButton;
    public Button refreshRoomList;
    public Button backToMenuFromJoin;
    public SocketIOComponent SocketIO;
    
    public Text joinStatus;


    public GameObject menu;
    public GameObject joinMenu;
    public GameObject createMenu;

    public GameObject itemListPrefab;
    public GameObject roomListParent;
    private List<GameObject> roomItemList = new List<GameObject>();
    public Button closeRoom;



    //private Button playButton;
    // Use this for initialization
    void Start () {

        joinMenu.SetActive(false);
        createMenu.SetActive(false);
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("roomIsFull", OnRoomIsFull);

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
            RefreshList();
        });

       
        refreshRoomList.onClick.AddListener(() =>
        {
            RefreshList();
        });
        backToMenuFromJoin.onClick.AddListener(() =>
        {
            createMenu.SetActive(false);
            joinMenu.SetActive(false);
            menu.SetActive(true);

        });
        exitButton.onClick.AddListener(() => {
            SocketIO.Emit("closeRoom");
            Application.Quit(); });

    }

    private void OnRoomIsFull(SocketIOEvent obj)
    {
        joinStatus.text = "Room is full. Refresh and chose another";
    }


    public void RefreshList()
    {
        joinStatus.text = "";
        if (NetworkRegisterLogin.RoomList.Count == 0)
        {
            joinStatus.text = "There is no room here";
        }

        for (int i = 0; i < roomItemList.Count; i++)
        {
            Destroy(roomItemList[i]);
        }
        roomItemList.Clear();

        foreach (KeyValuePair<string,Room> room in NetworkRegisterLogin.RoomList)
        {
            GameObject roomListItemGo = Instantiate(itemListPrefab);
            roomListItemGo.GetComponent<JoinGame>().SetupRoom(room.Value.nameRoom,room.Value.currentNumberOfPlayers,room.Value.maxNumberOfPlayers,room.Value.idRoom);
            roomItemList.Add(roomListItemGo);
            roomListItemGo.transform.SetParent(roomListParent.transform);
        }
    }



}
