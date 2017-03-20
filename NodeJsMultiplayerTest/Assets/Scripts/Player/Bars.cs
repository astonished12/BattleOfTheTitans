using UnityEngine;
using System.Collections;

public class Bars : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth = 100;

    public float healthBarLength;

    // Use this for initialization
    void Start()
    {
        healthBarLength = Screen.width / 6;
    }

    // Update is called once per frame
    void Update()
    {
        AddjustCurrentHealth(0);
    }

    void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);

        texture.Apply();
        GUI.skin.box.normal.background = texture;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);// gets screen position.
        screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
        GUI.color = Color.red;
        
        //Rect rect = new Rect(screenPosition.x - 50,   screenPosition.y-70, 100, 24);// makes a rect centered at the player ( 100x24 )
        GUI.Box(new Rect(screenPosition.x - 50, screenPosition.y - 70, 100, 24), "Health");
        //GUI.Box(rect, "Enemy");

    }

    public void AddjustCurrentHealth(int adj)
    {
        curHealth += adj;

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        healthBarLength = (Screen.width / 6) * (curHealth / (float)maxHealth);
    }
}