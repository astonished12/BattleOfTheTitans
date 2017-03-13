using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenClicker : MonoBehaviour {
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire2")){
            OnClick();
        }
      
	}
    private void OnClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            var clickEvent = hit.collider.gameObject.GetComponent<IClickable>();
            if(hit.collider.gameObject != null && clickEvent != null)
                clickEvent.OnClick(hit);            
        }
        
    }
}
