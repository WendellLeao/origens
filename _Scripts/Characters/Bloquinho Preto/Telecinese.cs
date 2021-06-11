using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Telecinese : MonoBehaviour
{
    public static Telecinese instance;
    private bool isActived = false;
    private bool isHeldingObject = false;
    CircleCollider2D circleCollider;
    private Color startColor;
    private float startWidthMultiplier;
    private bool canDamage = true;
    private bool isTouchingDragableObj = false;
    
    private void Awake()
    {
        instance = this;

        circleCollider = GetComponent<CircleCollider2D>();
        startColor = this.GetComponent<TrailRenderer>().startColor;

        startWidthMultiplier = GetComponent<TrailRenderer>().widthMultiplier;
    }
    void Update()
    {
        if(!BloquinhoBrancoController.instance.isBloquinhoBranco)
        {
            if(!GameHandler.IsGamePaused())
            {
                if(BloquinhoBrancoController.instance.dash != null)
                {
                    if (Input.GetMouseButton(0) && !BloquinhoBrancoController.instance.dash.IsDragging()) 
                        EnableTelecinese();
                    else 
                        DisableTelecinese();
                }
                else
                {
                    if (Input.GetMouseButton(0))
                        EnableTelecinese();
                    else 
                        DisableTelecinese();
                }
            }
            else
            {
                DisableTelecinese();
            }

            if(!isActived)
                canDamage = true;
        }
        else
        {
            DisableTelecinese();
        }

        if(isTouchingDragableObj && isActived)
            isHeldingObject = true;
    }
    void FixedUpdate()
    {
        PositionHandler();
    }

    private void PositionHandler()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
    private void EnableTelecinese()
    {
        isActived = true;

        if(MouseIsNotStopped(0.2f))
        {
            if(MouseIsNotStopped(0.3f))
            {
                CursorHandler.instance.HideCursor();

                //GetComponent<TrailRenderer>().widthMultiplier = startWidthMultiplier;
                GetComponent<TrailRenderer>().emitting = true;
                GetComponent<TrailRenderer>().time = 0.5f;
            }
            else
            {
                CursorHandler.instance.SetCursor(CursorHandler.instance.telecineseCursor);
                //GetComponent<TrailRenderer>().widthMultiplier = 0.5f;
            }
        }
        else
        {
            CursorHandler.instance.SetCursor(CursorHandler.instance.telecineseCursor);
            //GetComponent<TrailRenderer>().widthMultiplier = 0.5f;
        }
    }
    public void DisableTelecinese()
    {
        isActived = false;
        isHeldingObject = false;

        CursorHandler.instance.ShowCursor();
        CursorHandler.instance.ResetCursor();

        GetComponent<TrailRenderer>().time = 0.2f;
        GetComponent<TrailRenderer>().emitting = false;
    }

    public bool MouseIsNotStopped(float minimumInAxis)
    {
        return Input.GetAxis("Mouse X") >= minimumInAxis || Input.GetAxis("Mouse X") <= -minimumInAxis 
           ||  Input.GetAxis("Mouse Y") >= minimumInAxis || Input.GetAxis("Mouse Y") <= -minimumInAxis;
    }
    
    public bool IsActived()
    {
        return isActived;
    }
    public bool IsHeldingObject()
    {
        return isHeldingObject;
    }
    public Color StartColor
    {
        get{return startColor;}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if(damageable != null)
        {
            if(canDamage && isActived)
            {
                damageable.Damage();
                AudioHandler.instance.Play("HitEnemyFX");
                CinemachineShake.instance.ShakeCamera(3f, 0.2f);
                canDamage = false;
            }
        }

        DragAndDrop dragableObject = other.GetComponent<DragAndDrop>();

        if(dragableObject != null)
            isTouchingDragableObj = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(isActived)
            canDamage = true;
        
        DragAndDrop dragableObject = other.GetComponent<DragAndDrop>();
        
        if(dragableObject != null)
            isTouchingDragableObj = false;
    }

    void OnDisable()
    {
        DisableTelecinese();
    }
}
