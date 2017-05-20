using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChatUiScript : MonoBehaviour {

    public GameObject closeButton;
    public GameObject minimizeButton;
    public GameObject messagesContent;
    public GameObject messageInputField;

    public GameObject newMessagePrefab;
    public GameObject sendMessageButton;

    private RectTransform rect;
    Vector3 initialScale;
    private SocketIOComponent SocketIO;
    bool newMessage;
    string inputField = "";
    private string senderId;
    ArrayList entries;
    JSONParser myJsonParser = new JSONParser();

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialScale = gameObject.transform.localScale;
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("newMessageGlobalChat", OnMessageOnGlobalChat);

    }
    void AddChatEntry(string name, string msg, bool isMine)
    {
        ChatEntry newEntry = new ChatEntry();
        newEntry.name = name;
        newEntry.message = msg;
        newEntry.isMine = isMine;
        newEntry.timeTag = "[" + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + "]";
        entries.Add(newEntry);
    }

    private void OnMessageOnGlobalChat(SocketIOEvent Obj)
    {
        string socket_id = myJsonParser.ElementFromJsonToString(Obj.data["socket_id"].ToString())[1];
        string name_sender = myJsonParser.ElementFromJsonToString(Obj.data["name"].ToString())[1];
        string message = myJsonParser.ElementFromJsonToString(Obj.data["message"].ToString())[1];
        inputField = message;
        senderId = socket_id;
        if (newMessage)
        {
            AddChatEntry(name_sender, inputField, true);
        }
        else
        {
            AddChatEntry(name_sender, inputField, false);
        }
        newMessage = false;
    }

    public void OnCloseButtonPressed()
    {
        Destroy(gameObject);
    }

    public void OnMinimizeButtonPressed()
    {
        gameObject.transform.localScale /= 5f;      
    }

    public void OnMaximizeButtonPressed()
    {
        gameObject.transform.localScale = initialScale;
    }

    public void SendMessageButtonPressed()
    {
        Debug.Log("CE PLM");
        string message = messageInputField.GetComponent<InputField>().text;
        //to do add to messageScroll and send to node
        SocketIO.Emit("newMessageGlobalChat", new JSONObject(myJsonParser.MessageToPersonToJson(message, gameObject.transform.FindChild("To").GetComponent<Text>().text)));
    }

    public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
        if (pointerData == null) { return; }


        var currentPosition = rect.position;
        currentPosition.x += pointerData.delta.x;
        currentPosition.y += pointerData.delta.y;
        rect.position = currentPosition;
    }

    void UpdateChat()
    {
       foreach (ChatEntry ent in entries)
        {           
        GameObject newMessage = Instantiate(newMessagePrefab);
        newMessage.transform.SetParent(messagesContent.transform, false);
        if (ent.isMine)
            newMessage.GetComponent<Text>().color = Color.white;
        else
            newMessage.GetComponent<Text>().color = Color.green;

       newMessage.GetComponent<Text>().text = (ent.timeTag + " " + ent.name + ": " + ent.message);

        }
    }
}
