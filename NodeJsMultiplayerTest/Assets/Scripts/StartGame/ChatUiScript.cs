using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUiScript : MonoBehaviour {

    public GameObject closeButton;
    public GameObject minimizeButton;
    public GameObject messageScroll;
    public GameObject messageInputField;
    public GameObject sendMessageButton;
    private RectTransform rect;
    Vector3 initialScale;
    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialScale = gameObject.transform.localScale;
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
        Debug.Log(messageInputField.GetComponent<InputField>().text);
        //to do add to messageScroll and send to node
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

}
