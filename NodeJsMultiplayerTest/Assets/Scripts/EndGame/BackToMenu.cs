using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

    public void GoToMenu()
    {
        Debug.Log("CE PLM");
        SceneManager.LoadScene(2);
    }
}
