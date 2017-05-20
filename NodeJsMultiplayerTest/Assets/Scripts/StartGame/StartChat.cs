using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartChat : MonoBehaviour {
    public GameObject chatPrefab;
    public GameObject gameObjectParent;
    public void FriendOfflineOnClick(Button button)
    {
        var messageBox = Main.Assets.Scripts.Helpers.BringMessageBox();
        messageBox.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 35f);
        messageBox.SetMessage("Jucatorul nu este online");
    }

    public void FriendOnlineOnClick(Button button)
    {
        GameObject chat = Instantiate(chatPrefab);

        chat.transform.FindChild("To").GetComponent<Text>().text = button.transform.FindChild("Text").GetComponent<Text>().text;
        chat.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

   
}
