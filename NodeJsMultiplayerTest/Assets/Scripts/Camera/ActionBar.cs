using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {

    // Use this for initialization
    public Texture2D actionBar;
    public Rect position;
    public int numberOfSkills;
    public SkillSlot[] skillSlots;
    public SpecialAttack[] specialAttacks;
    public float skillX;
    public float skillY;
    public float skillWidth;
    public float skillHeight;
    public float skillDistance;

	void Start () {
        skillSlots = new SkillSlot[numberOfSkills+1];
        specialAttacks = GameObject.FindGameObjectWithTag("Player").GetComponents<SpecialAttack>();
        for (int i = 0; i < numberOfSkills; i++)
        {
            skillSlots[i] = new SkillSlot();
            skillSlots[i].skill = specialAttacks[i];
        }


        skillSlots[0].key = KeyCode.Q;
        skillSlots[1].key = KeyCode.W;
        skillSlots[2].key = KeyCode.E;
        skillSlots[3].key = KeyCode.R;


    }

    // Update is called once per frame
    void Update () {
        UpdateSkillSlots();
	}

    void UpdateSkillSlots()
    {
        for (int i = 0; i < numberOfSkills; i++)
        {
            skillSlots[i].position.Set(skillX + i * (skillWidth + skillDistance), skillY, skillWidth, skillHeight);
        }
    }
    void OnGUI()
    {
        DrawActionbar();
        DrawSkillSlots();
    }

    void DrawActionbar()
    {
        GUI.DrawTexture(getScreenRect(position), actionBar);
    }

    void DrawSkillSlots()
    {
        for(int i=0;i< numberOfSkills;i++)
        {
            GUI.DrawTexture(getScreenRect(skillSlots[i].position), skillSlots[i].skill.pictureSkill);
        }
    }

    Rect getScreenRect(Rect position)
    {
        return new Rect(Screen.width * position.x, Screen.height * position.y, Screen.width * position.width, Screen.height * position.height);
    }
}
