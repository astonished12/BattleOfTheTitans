using Main.Assets.Scripts;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class StartGame : MonoBehaviour {

    SocketIOComponent SocketIO;
    public GameObject usernameField;
    public GameObject passwordField;
    JSONParser myJsonParser = new JSONParser();

    private string Username;
    private string Password;
    void Start()
    {
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
        SocketIO.On("wrongData", OnDataDoesntExist);
        SocketIO.On("loginSuccesfull", OnLoginSuccesfull);
    }

    private void OnLoginSuccesfull(SocketIOEvent obj)
    {
        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = passwordField.transform.position;
        messageBox.SetMessage("Login succesfull.");
        NetworkRegisterLogin.loginSucccesfull = true;
        NetworkRegisterLogin.registedSuccesfull = false;
    }

    private void OnDataDoesntExist(SocketIOEvent obj)
    {
        var messageBox = Helpers.BringMessageBox();
        messageBox.transform.position = passwordField.transform.position;
        messageBox.SetMessage("Username or password wrong. Try again or new account.");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameField.GetComponent<InputField>().isFocused)
            {
                passwordField.GetComponent<InputField>().Select();
            }
            if (passwordField.GetComponent<InputField>().isFocused)
            {
                usernameField.GetComponent<InputField>().Select();
            }          

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginUser();
        }

        Username = usernameField.GetComponent<InputField>().text;
        Password = passwordField.GetComponent<InputField>().text;

    }

    //TO DO SEND LOGIN DATA TO SERVER
    public void LoginUser()
    {
        if(checkEmpty())
             SendLoginData(Username,Password);
        else
        {

            var messageBox = Helpers.BringMessageBox();
            messageBox.transform.position = passwordField.transform.position;
            messageBox.SetMessage("Username or password can not be empty");
        }
    }

    private bool checkEmpty()
    {
        return (Username != "" && Password != "");       
    }
  
	
    //BACK BUTTON
    public void ChangeLevelToRegister()
    {
        SceneManager.LoadScene(1);
    }
    public void SendLoginData(string username, string password)
    {
        SocketIO.Emit("login", new JSONObject(myJsonParser.LoginDataToJson(username, password)));
    }
}
