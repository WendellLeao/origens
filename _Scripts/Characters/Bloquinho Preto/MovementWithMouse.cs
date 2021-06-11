using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWithMouse : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Telecinese telecinese;

    [Header("Velocity")]
    [SerializeField] private float moveSpeed;
    private bool isFacingRight = true;

    [Header("Minimum to move")]
    public float minMouseDistX;
    public float minMouseDistY;
    private Vector2 mousePosition;

    [Header("Rope")]
    [SerializeField] private float maxDistance;
    private Transform bbPos;

    void Awake()
    {
        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(telecinese != null)
        {
            if(BloquinhoBrancoController.instance.dash != null)
            {
                if(!BloquinhoBrancoController.instance.dash.IsDragging())
                {
                    if (!telecinese.IsActived())
                    {
                        MovementHandler(moveSpeed);

                        // if(IsOutOfMinimumArea())
                        // {
                        //     MovementHandler(moveSpeed);
                        // }
                    }
                    else if(telecinese.IsActived() && telecinese.IsHeldingObject())
                    {
                        MovementHandler(moveSpeed / 1.5f);

                        // if(IsOutOfMinimumArea())
                        // {
                        //     MovementHandler(moveSpeed / 1.5f);
                        // }
                    }
                }
            }
            else
            {
                if (!telecinese.IsActived())
                {
                    MovementHandler(moveSpeed);

                    // if(IsOutOfMinimumArea())
                    // {
                    //     MovementHandler(moveSpeed);
                    // }
                }  
            }
        }
        else
        {
            MovementHandler(moveSpeed);
        }
        
        RopeLimitHandler();
        
        if(BloquinhoBrancoController.instance.dash != null)
        {
            if(!BloquinhoBrancoController.instance.dash.IsDragging())
                FlipHandler();
        }
        else
        {
            FlipHandler();
        }
    }
    private void MovementHandler(float moveSpeed)
    {
        //minMouseDistX = 2;
        //minMouseDistY = 2;
                    
        //transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);

        //if(BloquinhoBrancoController.instance.IsGrounded())
            transform.position = Vector3.Lerp (transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }
    public bool IsOutOfMinimumArea()
    {
        return mousePosition.x > transform.position.x + minMouseDistX || mousePosition.y > transform.position.y + minMouseDistY
        || mousePosition.x < transform.position.x - minMouseDistX || mousePosition.y < transform.position.y - minMouseDistY;
    }
    private void FlipHandler()
    {
        if (transform.position.x < mousePosition.x && !isFacingRight || transform.position.x > mousePosition.x && isFacingRight)
        {
            if(IsOutOfMinimumArea())
                Flip();
        }
    }
    private void RopeLimitHandler()
    {
        Vector3 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bbPos.position.x - maxDistance, bbPos.position.x + maxDistance);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bbPos.position.y - maxDistance, bbPos.position.y + maxDistance);

        transform.position = clampedPosition;
    }
    public void Flip()
    {
        isFacingRight = !isFacingRight;

        //GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        
        transform.localScale = theScale;
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    public float GetMoveSpeed()
    {
        return this.moveSpeed;
    }
}
