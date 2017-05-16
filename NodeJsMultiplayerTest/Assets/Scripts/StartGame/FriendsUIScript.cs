using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsUIScript : MonoBehaviour {

    public GameObject scrollDataObject;
    public GameObject findFriendButton;
    public GameObject searchTxtField;
    public GameObject welecomTextMessage;

    void Start()
    {
        welecomTextMessage.GetComponent<Text>().text = "Welcome " + NetworkRegisterLogin.UserName;
    }
    public void SearchAndAddButtonOnClick()
    {

    }
}
