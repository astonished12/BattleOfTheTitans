using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {

    private SocketIOComponent SocketIO;
    public static Dictionary<string, GameObject> chatList = new Dictionary<string, GameObject>();
    public GameObject chatPrefab;
    JSONParser myJsonParser = new JSONParser();

    private void Awake()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("newMessageGlobalChat", OnMessageOnGlobalChat);
    }

    

    private void OnMessageOnGlobalChat(SocketIOEvent Obj)
    {
        string socket_id = myJsonParser.ElementFromJsonToString(Obj.data["socket_id"].ToString())[1];
        string name_sender = myJsonParser.ElementFromJsonToString(Obj.data["name"].ToString())[1];
        string message = myJsonParser.ElementFromJsonToString(Obj.data["message"].ToString())[1];
        if (!chatList.ContainsKey(name_sender) && name_sender!=NetworkRegisterLogin.UserName){
            GameObject chat = Instantiate(chatPrefab);
            chat.transform.FindChild("To").GetComponent<Text>().text = name_sender;
            chat.transform.SetParent(gameObject.transform, false);
            chatList.Add(name_sender, chat);
        }

        chatList[name_sender].GetComponent<ChatUiScript>().inputField = message;

        chatList[name_sender].GetComponent<ChatUiScript>().senderId = socket_id;
        if (chatList[name_sender].GetComponent<ChatUiScript>().newMessage)
        {
            chatList[name_sender].GetComponent<ChatUiScript>().AddChatEntry(name_sender, chatList[name_sender].GetComponent<ChatUiScript>().inputField, true);
        }
        else
        {
            chatList[name_sender].GetComponent<ChatUiScript>().AddChatEntry(name_sender, chatList[name_sender].GetComponent<ChatUiScript>().inputField, false);
        }
        chatList[name_sender].GetComponent<ChatUiScript>().newMessage = false;
        
    }
}
