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
        messageBox.SetMessage(string.Format("You:{0}\n Oponent:{1} \n Owner:{2} \n Status:{3} \n TotalDmg:{4} \n" , playerName,oponentName, CheckOwner(owner), CheckVictory(status), totalDamage));
    }
    public string CheckOwner(string status)
    {
        if (status=="1")
        {
            return "True";
        }
        else
        {
            return "False";
        }
    }
    public string CheckVictory(string status)
    {
        if (status=="1")
        {
            return "Victory";
        }
        else
        {
            return "Lose";
        }
    }
}   

