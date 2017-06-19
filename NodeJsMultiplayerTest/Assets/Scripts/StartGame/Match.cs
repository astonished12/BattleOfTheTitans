using Main.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {

    public string playerName;
    public string oponentName;
    public string owner;
    public string totalDamage;
    public string status;

    public void OnClick()
    {

        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position+new Vector3(0f,0f,20f);
        messageBox.SetMessage(string.Format("Your name {0}\n Oponent name {1}" , playerName,oponentName));
    }
}
