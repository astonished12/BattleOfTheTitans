﻿using Main.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

    public void GoToMenu()
    {
        SceneManager.LoadScene(2);
    }
}
