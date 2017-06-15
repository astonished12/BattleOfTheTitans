using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour {

    private Vector2 posOfHealthBar = new Vector2(Screen.width*0.8f, Screen.height*0.05f);
    private Vector2 size = new Vector2(Screen.width*0.18f, Screen.height*0.08f);
    public static int kills = 0;
    public static int deaths = 0;
    public static int assits = 0;
    public static int minions = 0;

    void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Box(new Rect(posOfHealthBar.x, posOfHealthBar.y, size.x, size.y), "K/D/A/M \n " + kills + '/'+ deaths + '/'+ assits+'/'+ minions + '\n');
        GUI.contentColor = Color.green;
        GUI.Box(new Rect(posOfHealthBar.x, posOfHealthBar.y+size.y, size.x, size.y), "Health/Mana \n" + GameObject.FindGameObjectWithTag("Player").GetComponent<Alive>().curHealth+ '/' + GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>().curMana + '\n');

    }

}
