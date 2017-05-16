using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


class ChatEntry
{
    public string name = "";
    public string message = "";
    public string timeTag = "";
    public bool isMine;
}

public class Chat : MonoBehaviour
{
   
    ArrayList entries;
    Vector2 currentScrollPos = new Vector2();
    private Rect chatRect = new Rect(Screen.width*0.01f, Screen.height * 0.45f, Screen.width * 0.3f, Screen.height * 0.3f);

    string inputField = "";
    bool chatInFocus = false;
    string inputFieldFocus = "CIFT";
    private NetworkCommunication netCommunication;
    private bool newMessage;
    private string senderId;
    public static SocketIOComponent SocketIO;

    JSONParser myJsonParser = new JSONParser();
    void Awake()
    {
        netCommunication = GetComponent<NetworkCommunication>();
        InitializeChat();
    }

    void Start()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("newMessage", OnMessageOnGameChat);

    }
    void InitializeChat()
    {
        entries = new ArrayList();    
        unfocusChat();
    }
    private void OnGUI()
    {
        Draw();
    }
    //draw the chat box in size relative to your GUIlayout
    public void Draw()
    {
        ChatWindow();
    }

    void ChatWindow()
    {
        GUILayout.BeginArea(chatRect);
        GUILayout.BeginVertical("box");
        currentScrollPos = GUILayout.BeginScrollView(currentScrollPos);
        GUILayout.FlexibleSpace();
        foreach (ChatEntry ent in entries)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.wordWrap = true;
            if(ent.isMine)
                GUI.contentColor = Color.white;
            else
                GUI.contentColor = Color.green;

            GUILayout.Label(ent.timeTag + " " + ent.name + ": " + ent.message);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }
        GUILayout.EndScrollView();
        if (chatInFocus)
        {
            GUI.SetNextControlName(inputFieldFocus);
            inputField = GUILayout.TextField(inputField, GUILayout.MaxWidth(Screen.width * 0.3f), GUILayout.MinWidth(Screen.height * 0.3f));
            GUI.FocusControl(inputFieldFocus);
        }
        GUILayout.EndVertical();

        GUILayout.EndArea();

        if (chatInFocus)
        {
            HandleNewEntries();
        }
        else
        {
            checkForInput();
        }

    }

    void unfocusChat()
    {
        inputField = "";
        senderId = "";
        chatInFocus = false;
    }

    void checkForInput()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.character == '\n'  && !chatInFocus){
            GUI.FocusControl(inputFieldFocus);
            chatInFocus = true;
            currentScrollPos.y = float.PositiveInfinity;
            inputField = "";
        }
    }

    void HandleNewEntries()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.character == '\n'){
            if (inputField.Length <= 0)
            {
                unfocusChat();
                Debug.Log("unfocusing chat (empty entry)");
                return;
            }
            newMessage = true;
            netCommunication.SendMessageChat(inputField);
            unfocusChat();
        }
    }
    
   
    void AddChatEntry(string name, string msg,bool isMine)
    {
        ChatEntry newEntry = new ChatEntry();
        newEntry.name = name;
        newEntry.message = msg;
        newEntry.isMine = isMine;
        newEntry.timeTag = "[" + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + "]";
        entries.Add(newEntry);
        currentScrollPos.y = float.PositiveInfinity;
    }

  

    private void OnMessageOnGameChat(SocketIOEvent Obj)
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
}

