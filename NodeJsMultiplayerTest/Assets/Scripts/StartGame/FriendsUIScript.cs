﻿using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Main.Assets.Scripts;

public class FriendsUIScript : MonoBehaviour {

    public GameObject contentParent;
    public GameObject findFriendButton;
    public GameObject searchTxtField;
    public GameObject welecomTextMessage;
    public GameObject friendPrefab;

    private SocketIOComponent SocketIO;
    private List<GameObject> friendList = new List<GameObject>();

    void Start()
    {
        welecomTextMessage.GetComponent<Text>().text = "Welcome " + NetworkRegisterLogin.UserName;
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("playerNotOnline", OnPlayerIsntOnline);
        SocketIO.On("newFriend", OnNewRequestFriendShip);
    }

    private void OnNewRequestFriendShip(SocketIOEvent obj)
    {
        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
        messageBox.SetMessage("New reqeust");

        JSONParser myJsonParser = new JSONParser();
        var friendName = myJsonParser.ElementFromJsonToString(obj.data.GetField("name").ToString())[1];
        GameObject newFriend = Instantiate(friendPrefab);
        newFriend.transform.FindChild("Text").GetComponent<Text>().text = friendName;
        newFriend.transform.SetParent(contentParent.transform,false);
        friendList.Add(newFriend);

    }

    private bool CheckIfFriendAlreadyInList(string friendName)
    {
        foreach(GameObject frind in friendList)
        {
            if (frind.transform.FindChild("Text").GetComponent<Text>().text == friendName)
                return true;
        }
        return false;
    }
    private void OnPlayerIsntOnline(SocketIOEvent obj)
    {
        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
        messageBox.SetMessage("Playr is not online");
    }

    public void SearchAndAddButtonOnClick()
    {
        string playerName = searchTxtField.GetComponent<InputField>().text;
        JSONParser myJsonParser = new JSONParser();
        if (playerName != null)
        {
            if (CheckIfFriendAlreadyInList(playerName) == false && playerName != NetworkRegisterLogin.UserName)
            {
                SocketIO.Emit("searchFriend", new JSONObject(myJsonParser.FriendNameToJson(playerName)));            
                GameObject newFriend = Instantiate(friendPrefab) as GameObject;
                newFriend.transform.FindChild("Text").GetComponent<Text>().text = playerName;
                newFriend.transform.SetParent(contentParent.transform, false);
                friendList.Add(newFriend);
            }
            else
            {
                var messageBox = Helpers.BringMessageBox();
                messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
                messageBox.SetMessage("Jucatorul exista in lista");
            }
        }
       
    }
}
