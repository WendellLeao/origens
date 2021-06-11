using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragAndDrop : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected float dragSpeed;

    [Header("Outline")]
    [SerializeField] private GameObject outlineSpriteObj;
    protected bool isBeingHeld = false;
    protected bool isBeingPressed = false;
    protected Rigidbody2D body;
    private bool isTouchingTelecinese = false;
    private Vector3 mousePos;
    private bool canMove = true;
        
    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            isBeingHeld = false;
            isBeingPressed = false;

            outlineSpriteObj.SetActive(false);
        }
        else
        {
            if(Input.GetMouseButton(0) && isTouchingTelecinese)
            {
                isBeingPressed = true;
                isBeingHeld = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isBeingPressed = false;
                isBeingHeld = false;
            }

            OutlineHandler();
        }
    }
    protected virtual void FixedUpdate()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (isBeingHeld && canMove)
        {
            Vector3 newPos = new Vector3(mousePos.x, transform.position.y, 0);
            body.MovePosition(Vector3.Lerp(transform.position, newPos, Time.fixedDeltaTime * dragSpeed));
        }
    }

    private void OutlineHandler()
    {
        if(Time.deltaTime > 0f)
        {
            if(!isBeingHeld && isTouchingTelecinese)
            {
                outlineSpriteObj.SetActive(true);
            }
            else
            {
                outlineSpriteObj.SetActive(false);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Mouse")
            isTouchingTelecinese = true;
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Mouse")
            isTouchingTelecinese = false;
    }
}
