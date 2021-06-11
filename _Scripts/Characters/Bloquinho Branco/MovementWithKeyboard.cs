using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovementWithKeyboard : MonoBehaviour
{
    public static event Action onPlayerFlip;

    [Header("Components")]
    private Rigidbody2D body;

    [Header("Player's Velocity")]
    [SerializeField] private float walkSpeed;
    private float moveInputHorizontal;
    private float startWalkSpeed;

    [Header("Slope")]
    [SerializeField] private PhysicsMaterial2D withFriction;
    [SerializeField] private PhysicsMaterial2D noFriction;

    [Header("Move With Movable Platforms")]
    [SerializeField] private LayerMask whatIsMovingPlatform;

    [Header("Others")]
    [HideInInspector] public bool isFacingRight = true;
    private bool isOnPlatform = false;
    
    void Awake()
    {
        startWalkSpeed = walkSpeed;

        body = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Debug.Log(walkSpeed);

        moveInputHorizontal = Input.GetAxis("Horizontal");

        if (moveInputHorizontal != 0)
        {
            body.sharedMaterial = noFriction;
                
            if(GetComponent<BloquinhoBrancoController>().IsGrounded())
            {
                //CreateDust();

                if(GetComponent<Sprint>() != null)
                {
                    if(!GetComponent<Sprint>().IsRunning())
                    {
                        if(body.velocity.y < 1f)
                        {
                            if (GetComponent<Sprint>().IsDiminished)
                                GetComponent<AnimationHandler>().PlayAnimation("BBWalkAnim");
                            else
                                GetComponent<AnimationHandler>().PlayAnimation("RunToWalkAnim");
                        }
                    }
                }
            }
        } 
        else 
        {
            body.sharedMaterial = withFriction;
            
            // if(GetComponent<BloquinhoBrancoController>().IsGrounded() || !GetComponent<BloquinhoBrancoController>().IsGrounded() && Mathf.Abs(body.velocity.y) <= 0.000001f)
            //     GetComponent<AnimationHandler>().PlayAnimation("BBIdleAnim");

            if(GetComponent<BloquinhoBrancoController>().IsGrounded() || !GetComponent<BloquinhoBrancoController>().IsGrounded() && Mathf.Abs(body.velocity.y) <= 0.000001f)
            {
                if(GetComponent<Sprint>().IsDiminished)
                    GetComponent<AnimationHandler>().PlayAnimation("BBIdleAnim");
            }
        }

        //MovementSpeedOnMovingPlatformHandler();
        FlipHandler();
    }
    void FixedUpdate()
    {
        if(GetComponent<Sprint>() != null)
        {
            if(!GetComponent<Sprint>().IsRunning())
                MovementHandler(walkSpeed);
        }
        else
        {
            MovementHandler(walkSpeed);
        }
    }

    public void MovementHandler(float moveSpeed)
    {        
        body.velocity = new Vector2(moveInputHorizontal * moveSpeed, body.velocity.y);
    }

    private void MovementSpeedOnMovingPlatformHandler()
    {
        if(BloquinhoBrancoController.instance.IsOnMovingPlatform()) 
        {
            walkSpeed = walkSpeed - 2f;
        }
        else
        {
            walkSpeed = startWalkSpeed;
        }
    }

    public void Skid()
    {
        if(GetComponent<Sprint>() != null)
        {
            if(GetComponent<Sprint>().CanSprintInTheAir())
            {
                if(!GetComponent<BloquinhoBrancoController>().IsGrounded())
                {
                    body.velocity = new Vector2(body.velocity.x * 0.98f, body.velocity.y);
                }
                else
                {
                    body.velocity = new Vector2(body.velocity.x * 0.96f, body.velocity.y);
                    //GetComponent<BloquinhoBrancoController>().CreateDust();
                }
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x * 0.88f, body.velocity.y);
            }
        }
    }
    public void FlipHandler()
    {
        if(moveInputHorizontal > 0 && !isFacingRight || moveInputHorizontal < 0 && isFacingRight) 
            Flip();
    }
    public void Flip()
    {
        onPlayerFlip?.Invoke();

        isFacingRight = !isFacingRight;
        
        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        
        transform.localScale = theScale;

        if(GetComponent<DamageHandler>() != null)
            FlipHealthBar();

        if(GetComponent<BloquinhoBrancoController>() != null && GetComponent<BloquinhoBrancoController>().IsGrounded() 
            && !BloquinhoBrancoController.instance.IsOnMovingPlatform())//!IsOnPlatform()
            GetComponent<BloquinhoBrancoController>().CreateDust();
    }

    private void FlipHealthBar()
    {
        Vector3 healthBarTransformScale = GetComponent<DamageHandler>().healthBarTransform.localScale;

        healthBarTransformScale.x *= -1;
        
        GetComponent<DamageHandler>().healthBarTransform.localScale = healthBarTransformScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        WayPointSystem wayPointObject = other.GetComponent<WayPointSystem>();
        
        if(wayPointObject != null)
        {
            if(BloquinhoBrancoController.instance.IsGrounded())
                isOnPlatform = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        WayPointSystem wayPointObject = other.GetComponent<WayPointSystem>();
        
        if(wayPointObject != null)
            isOnPlatform = false;
    }
    
    // public bool IsOnPlatform()
    // {
    //     // RaycastHit2D platInfo = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, 0.3f, whatIsMovingPlatform);
        
    //     // return platInfo.collider != null;
    //     return isOnPlatform;
    // }
    
    public float GetWalkSpeed()
    {
        return walkSpeed;
    }
    public void SetWalkSpeed(float newSpeed)
    {
        this.walkSpeed = newSpeed;
    }
    public float GetStartWalkSpeed()
    {
        return startWalkSpeed;
    }
    public float GetMoveInputHorizontal()
    {
        return moveInputHorizontal;
    }
    public bool IsFacingRight()
    {
        return isFacingRight;
    }
}

