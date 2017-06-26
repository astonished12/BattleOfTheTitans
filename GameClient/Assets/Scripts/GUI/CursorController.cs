using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public Texture2D cursorMouseMove;
    public CursorMode cursorMode = CursorMode.Auto;

    public  void ChangeToMoveCursor()
    {
        Cursor.SetCursor(cursorMouseMove, Vector2.zero, cursorMode);
        StartCoroutine(ChangeCursorIntial());
    }

    public  IEnumerator ChangeCursorIntial()
    {
        yield return new WaitForSeconds(0.5f);
        Cursor.SetCursor(null, Vector2.zero, cursorMode);

    }
}
