using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {

    private SocketIOComponent SocketIO;
    public static Dictionary<string, GameObject> chatList = new Dictionary<string, GameObject>();
    public static Dictionary<string,    ArrayList> entries = new Dictionary<string, ArrayList>();
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
        string listener = myJsonParser.ElementFromJsonToString(Obj.data["name"].ToString())[1];
        string message = myJsonParser.ElementFromJsonToString(Obj.data["message"].ToString())[1];
        if (!chatList.ContainsKey(listener)){
            GameObject chat = Instantiate(chatPrefab);
            chat.transform.FindChild("To").GetComponent<Text>().text = listener;
            chat.transform.SetParent(gameObject.transform, false);
            chatList.Add(listener, chat);
        }

        chatList[listener].GetComponent<ChatUiScript>().inputField = message;

        chatList[listener].GetComponent<ChatUiScript>().senderId = socket_id;
        if (chatList[listener].GetComponent<ChatUiScript>().newMessage)
        {
            chatList[listener].GetComponent<ChatUiScript>().AddChatEntry(NetworkRegisterLogin.UserName, chatList[listener].GetComponent<ChatUiScript>().inputField, true);
        }
        else
        {
            chatList[listener].GetComponent<ChatUiScript>().AddChatEntry(listener, chatList[listener].GetComponent<ChatUiScript>().inputField, false);
        }
        chatList[listener].GetComponent<ChatUiScript>().newMessage = false;
        
    }
}
