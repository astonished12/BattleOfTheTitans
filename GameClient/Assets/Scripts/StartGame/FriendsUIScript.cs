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
    public GameObject removeFriendButton;
    public GameObject searchTxtField;
    public GameObject searchTxtFieldForRemove;
    public GameObject welecomTextMessage;
    public GameObject friendPrefabOnline;
    public GameObject friendPrefabOffline;

    private SocketIOComponent SocketIO;
    private Dictionary<string, GameObject> friendList = new Dictionary<string, GameObject>();
    JSONParser myJsonParser = new JSONParser();

    void Start()
    {
        welecomTextMessage.GetComponent<Text>().text = "Welcome " + NetworkRegisterLogin.UserName;
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.Emit("getMyFriends", new JSONObject(myJsonParser.MessageToJson(NetworkRegisterLogin.UserName)));
        SocketIO.On("playerNotOnline", OnPlayerIsntOnline);
        SocketIO.On("newFriend", OnNewRequestFriendShip);
        SocketIO.On("removeFriend", OnRemoveFriend);
        SocketIO.On("listFriends", OnReceiveListFriends);
        SocketIO.On("updateListFriends", OnUpateListFriends);
    }

    private void OnUpateListFriends(SocketIOEvent obj)
    {
        SocketIO.Emit("getMyFriends", new JSONObject(myJsonParser.MessageToJson(NetworkRegisterLogin.UserName)));
    }

    private void OnReceiveListFriends(SocketIOEvent obj)
    {
        DestroyAllFriendsGameObjects();
        friendList = new Dictionary<string, GameObject>();

        JSONObject friends = obj.data.GetField("friends");
        for(int i=0;i<friends.Count; i++)
        {
            string username = myJsonParser.ElementFromJsonToString(friends[i].GetField("username").ToString())[1];
            string isOnline = friends[i].GetField("isOnline").ToString().Replace("\"","");
            if (isOnline == "0")
            {
                GameObject newFriend = Instantiate(friendPrefabOffline);
                newFriend.transform.FindChild("Text").GetComponent<Text>().text = username;
                newFriend.transform.SetParent(contentParent.transform, false);
                friendList.Add(username, newFriend);
            }
            else if(isOnline == "1")
            {
                GameObject newFriend = Instantiate(friendPrefabOnline);
                newFriend.transform.FindChild("Text").GetComponent<Text>().text = username;
                newFriend.transform.SetParent(contentParent.transform, false);
                friendList.Add(username, newFriend);
            }
        }
    }

    private void DestroyAllFriendsGameObjects()
    {
        foreach(string key in friendList.Keys)
        {
            Destroy(friendList[key]);
        }
    }

    private void OnNewRequestFriendShip(SocketIOEvent obj)
    {
        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
        messageBox.SetMessage("New reqeust");
        SocketIO.Emit("getMyFriends", new JSONObject(myJsonParser.MessageToJson(NetworkRegisterLogin.UserName)));
    }

    private void OnRemoveFriend(SocketIOEvent obj)
    {
        JSONParser myJsonParser = new JSONParser();
        var friendName = myJsonParser.ElementFromJsonToString(obj.data.GetField("name").ToString())[1];

        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
        messageBox.SetMessage(friendName+" remove you.");
        SocketIO.Emit("getMyFriends", new JSONObject(myJsonParser.MessageToJson(NetworkRegisterLogin.UserName)));
    }

    private bool CheckIfFriendAlreadyInList(string friendName)
    {
        foreach(string key in friendList.Keys)
        {
            if (key == friendName)
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

    public void RemoveFriendButtonOnClick()
    {
        string playerName = searchTxtFieldForRemove.GetComponent<InputField>().text;
        JSONParser myJsonParser = new JSONParser();
        if (playerName != null)
        {
            if (CheckIfFriendAlreadyInList(playerName) == true)
            {
                SocketIO.Emit("removeFriend", new JSONObject(myJsonParser.FriendNameToJson(playerName)));
            }
            else
            {
                var messageBox = Helpers.BringMessageBox();
                messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
                messageBox.SetMessage("Jucatorul nu exista in lista");
            }
        }
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
