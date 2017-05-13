using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class ChatEntry
{
    public string name = "";
    public string message = "";
    public string timeTag = "";
}

public class Chat : MonoBehaviour
{
   
    ArrayList entries;
    Vector2 currentScrollPos = new Vector2();
    private Rect chatRect = new Rect(Screen.width*0.05f, Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.3f);

    string inputField = "";
    bool chatInFocus = false;
    string inputFieldFocus = "CIFT";

    void Awake()
    {
        InitializeChat();
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
            GUILayout.Label(ent.timeTag + " " + ent.name + ": " + ent.message);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }
        GUILayout.EndScrollView();
        if (chatInFocus)
        {
            GUI.SetNextControlName(inputFieldFocus);
            inputField = GUILayout.TextField(inputField, GUILayout.MaxWidth(400), GUILayout.MinWidth(400));
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
        chatInFocus = false;
    }

    void checkForInput()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.character == '\n'  && !chatInFocus){
            GUI.FocusControl(inputFieldFocus);
            chatInFocus = true;
            currentScrollPos.y = float.PositiveInfinity;
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
            //networkView.RPC("AddChatEntry", RPCMode.All, "Cookie monster", inputField);
            AddChatEntry("Cookie monster", inputField); //for offline testing
            unfocusChat();
            //Debug.Log("unfocusing chat and entry sent");
        }
    }
    
   
    void AddChatEntry(string name, string msg)
    {
        ChatEntry newEntry = new ChatEntry();
        newEntry.name = name;
        newEntry.message = msg;
        newEntry.timeTag = "[" + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + "]";
        entries.Add(newEntry);
        currentScrollPos.y = float.PositiveInfinity;
    }
}

