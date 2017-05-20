using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCanvas : MonoBehaviour {

    public GameObject canvasVictory;
    public GameObject canvasDefeat;
    private void Awake()
    {
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
    }

    public void MoveToMenu()
    {
        Debug.Log("Move to menu");
    }
}
