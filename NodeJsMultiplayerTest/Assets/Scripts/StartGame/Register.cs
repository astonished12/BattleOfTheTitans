using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour {

    public GameObject username;
    public GameObject email;
    public GameObject password;

    private string Username;
    private string Email;
    private string Password;

    public void ChangeLevelToLogin()
    {
        SceneManager.LoadScene(0);
    }

   // public void 
    public void RegisterUser()
    {

    }
}
