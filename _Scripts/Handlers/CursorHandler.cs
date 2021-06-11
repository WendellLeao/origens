using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    public static CursorHandler instance;

    [Header("Cursor Textures")]
    public Texture2D blackArrow;
    public Texture2D telecineseCursor;
    
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(BloquinhoBrancoController.instance.isBloquinhoBranco)    
            ResetCursor();
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }
    public void HideCursor()
    {
        Cursor.visible = false;
    }

    public void SetCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
