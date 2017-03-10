using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToClick : MonoBehaviour,IClickable{
    public void OnClick(RaycastHit hit)
    {
        Debug.Log("Following " + hit.collider.gameObject.name);
    }

   
}
