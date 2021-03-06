﻿using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour {

    // Use this for initialization
    private GameObject[] characterList;
    public int index = 0;
    private SocketIOComponent SocketIO;
    public GameObject panelConfirm;

    void Start () {
        panelConfirm.SetActive(false);
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        characterList = new GameObject[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }
        foreach(GameObject go in characterList)
        {
            go.SetActive(false);
        }
        if (characterList[0])
            characterList[0].SetActive(true); 
    }
	
	// Update is called once per frame
	public void Next()
    {
        characterList[index].SetActive(false);
        if (index == characterList.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        characterList[index].SetActive(true);

    }

    public void Previous()
    {
        characterList[index].SetActive(false);
        if (index == 0)
        {
            index = characterList.Length-1;
        }
        else
        {
            index--;
        }
        characterList[index].SetActive(true);

    }

    public void Confirm()
    {
        panelConfirm.SetActive(true);
        NetworkRegisterLogin.noCharacter = index;
        SocketIO.Emit("characterId", new JSONObject(CharacterIdToJson(index.ToString())));
    }

    private string CharacterIdToJson(string id)
    {
        return string.Format(@"{{""idCharacter"":""{0}""}}", id);
    }
}
