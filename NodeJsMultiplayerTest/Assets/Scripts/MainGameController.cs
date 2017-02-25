using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;

public class MainGameController : MonoBehaviour
{

    public SocketIOComponent SocketIO;
    public GameObject character1;
    public GameObject character2;

    public GameObject MainChracter;
    public GameObject[] OtherPlayers= new GameObject[20];
    // Use this for initialization
    void Start()
    {
        SocketIO.On("identify", OnIdentify);
        SocketIO.On("anotherplayerconnected", OtherPlayer);
        //SocketIO.On("playerLeft", OnPlayerLeft);
        //socket.on("handleKeys", playerMove);
        //StartCoroutine (ConnectToServer ());
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnIdentify(SocketIOEvent Obj)
    {
        //TO DO CREATE DYNAMICALLY AN OBJECT
        MainChracter = Instantiate(character1, GetVectorPositonFromJson(Obj.data), Quaternion.identity);
        Debug.Log("Second Commit Test");

        var players = Obj.data.GetField("allPlayersAtCurrentTime");
        int k = 0;
        for (int i = 0; i < players.list.Count; i++)
        {
            string playerKey = (string)players.keys[i];
            JSONObject playerData = (JSONObject)players.list[i];
            // Process the player key and data as you need.
            GameObject Player = Instantiate(character2, GetVectorPositonFromJson(playerData), Quaternion.identity);
            OtherPlayers[k++] = Player;

        }
        for(int i = 0; i < OtherPlayers.Length; i++)
        {
            print(OtherPlayers[i].gameObject);
        }

    }

    string[] ElementFromJsonToString(string target) {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }


    Vector3 GetVectorPositonFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString()), float.Parse(Json["y"].ToString()), float.Parse(Json["z"].ToString()));
    }

    void OtherPlayer(SocketIOEvent Obj)
    {

    }

}