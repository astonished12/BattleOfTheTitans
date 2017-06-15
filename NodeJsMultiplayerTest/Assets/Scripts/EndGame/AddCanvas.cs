using Main.Assets.Scripts;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCanvas : MonoBehaviour {

    public GameObject canvasVictory;
    public GameObject canvasDefeat;
    public static SocketIOComponent SocketIO;
    private void Awake()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        AddCanvasToCamera(NetworkRegisterLogin.lastMatch);
    }
    public void AddCanvasToCamera(bool status)
    {
        if (status)
        {
            GameObject canvas = Instantiate(canvasVictory);
            canvas.transform.SetParent(Camera.main.transform, false);
        }
        else
        {
            GameObject canvas = Instantiate(canvasDefeat);
            canvas.transform.SetParent(Camera.main.transform, false);
        }

        MessageBox messageBox;
        switch (NetworkRegisterLogin.status)
        {
            case "ihack":
                messageBox = Helpers.BringMessageBox();
                messageBox.transform.position = Camera.main.transform.position + new Vector3(0f,0f,20f);                
                messageBox.SetMessage("You are a hacker");
                break;
            case "youhack":
                messageBox = Helpers.BringMessageBox();
                messageBox.transform.position = Camera.main.transform.position+ new Vector3(0f, 0f, 20f);
                messageBox.SetMessage("Hack detected on the other side");
                break;
            default:
                break;

        }
        SocketIO.Emit("closeRoom");
    }

    public void MoveToMenu()
    {
        Debug.Log("Move to menu");
    }
}
