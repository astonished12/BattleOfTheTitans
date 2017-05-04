using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot 
{
    public SpecialAttack skill;
    public Rect positionRect;
    public KeyCode key;

    public bool coolDownActive=false;
    public float coolDonwTime;
 }
