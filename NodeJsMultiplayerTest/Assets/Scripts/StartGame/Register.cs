using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Main.Assets.Scripts;

public class Register : MonoBehaviour {

    public GameObject usernameField;
    public GameObject emailField;
    public GameObject passwordField;

    private string Username;
    private string Email;
    private string Password;
    private string form;
    private bool EmailValid = false;



    public void ChangeLevelToLogin()
    {
        SceneManager.LoadScene(0);
    }

    public void SendDataToServer()
    {

    }
    public void RegisterUser()
    {
        if (ValidationInputs())
            Debug.Log("GOOD REGISTER");
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameField.GetComponent<InputField>().isFocused)
            {
                passwordField.GetComponent<InputField>().Select();
            }
            if (passwordField.GetComponent<InputField>().isFocused)
            {
                emailField.GetComponent<InputField>().Select();
            }
            if (emailField.GetComponent<InputField>().isFocused)
            {
                usernameField.GetComponent<InputField>().Select();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RegisterUser();
        }

        Username = usernameField.GetComponent<InputField>().text;
        Email = emailField.GetComponent<InputField>().text;
        Password = passwordField.GetComponent<InputField>().text;

    }

    private bool ValidationInputs()
    {
         string errorMessage;
        if (!ValidatePassword(Password, out errorMessage)) {            
            var messageBox = Helpers.BringMessageBox();
            messageBox.transform.position = passwordField.transform.position;
            messageBox.SetMessage(errorMessage);
            return false;
        }

        if(!ValidateUser(Username,out errorMessage))
        {
            var messageBox = Helpers.BringMessageBox();
            messageBox.transform.position = passwordField.transform.position;
            messageBox.SetMessage(errorMessage);
            return false;
        }
        if (!ValidateEmail(Email, out errorMessage))
        {
            var messageBox = Helpers.BringMessageBox();
            messageBox.transform.position = passwordField.transform.position;
            messageBox.SetMessage(errorMessage);
            return false;
        }

        return true;

    }
           
    


    private bool ValidatePassword(string password, out string ErrorMessage)
    {
        var input = password;
        ErrorMessage = string.Empty;

        if (String.IsNullOrEmpty(input))
        {
            ErrorMessage = "Password should not be empty";
            return false;
        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(input))
        {
            ErrorMessage = "Password should contain At least one lower case letter";
            return false;
        }
        else if (!hasUpperChar.IsMatch(input))
        {
            ErrorMessage = "Password should contain At least one upper case letter";
            return false;
        }
        else if (input.Length > 12 && input.Length < 9)
        {
            ErrorMessage = "Password should not be less than 8 or greater than 12 characters";
            return false;
        }
        else if (!hasNumber.IsMatch(input))
        {
            ErrorMessage = "Password should contain At least one numeric value";
            return false;
        }
        else if (!hasSymbols.IsMatch(input))
        {
            ErrorMessage = "Password should contain At least one special case characters";
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool ValidateUser(string user, out string ErrorMessage)
    {
        var input = user;
        ErrorMessage = string.Empty;
        if (String.IsNullOrEmpty(input))
        {
            ErrorMessage = "User should not be empty";
            return false;
        }
        return true;
    }

    private bool ValidateEmail(string email, out string ErrorMessage)
    {
        var input = email;
        ErrorMessage = string.Empty;
        var isEmail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

        if (String.IsNullOrEmpty(input))
        {
            ErrorMessage = "Email should not be empty";
            return false;
        }
        else if (!isEmail.IsMatch(input))
        {
            ErrorMessage = "Email wrong format (x@x.x)";
            return false;
        }

        return true;
    }
}
