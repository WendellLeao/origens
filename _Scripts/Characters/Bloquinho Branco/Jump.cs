using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Jump : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D body;
    BoxCollider2D boxCollider2D;

    [Header("Player's Jump")]
    [SerializeField] private float jumpForce;
    private float startJumpForce;
    private bool isJumping = false;

    [Header("Check Ground")]
    [SerializeField] private LayerMask whatIsGround;
    private WayPointSystem lastMovingPlatform;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        startJumpForce = jumpForce;
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Jump") && GetComponent<BloquinhoBrancoController>().IsGrounded() && body.velocity.y < 1f)
        {
            isJumping = true;
        }

        if(!GetComponent<BloquinhoBrancoController>().IsGrounded() && GetComponent<Sprint>().CanSprintInTheAir() && StaminaSystem.instance.GetStaminaAmount() > 0f)
            GetComponent<AnimationHandler>().PlayAnimation("BBSprintJumpAnim");
    }
    void FixedUpdate()
    {
        if(isJumping)
        {
            if(GetComponent<BloquinhoBrancoController>() != null)
                GetComponent<BloquinhoBrancoController>().CreateDust();

            if(AudioHandler.instance != null)
                AudioHandler.instance.Play("PlayerJumpFX");
            
            body.velocity = new Vector2(body.velocity.x, 0f);////////////
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            isJumping = false;
        }

        JumpForceOnMovingPlatformHandler();

        if (body.velocity.y != 0 && !GetComponent<BloquinhoBrancoController>().IsGrounded())
        {
            // if(GetComponent<Sprint>().IsDiminished)
            //     GetComponent<AnimationHandler>().PlayAnimation("BBJumpAnim");
            // else
            //     GetComponent<AnimationHandler>().PlayAnimation("BBSprintJumpAnim");

            if (GetComponent<Sprint>().IsDiminished)
            {
                GetComponent<AnimationHandler>().PlayAnimation("BBJumpAnim");
            }
            else
            {
                if(Mathf.Abs(GetComponent<MovementWithKeyboard>().GetMoveInputHorizontal()) >= 0.5f)
                    GetComponent<AnimationHandler>().PlayAnimation("BBSprintJumpAnim");
                else
                    GetComponent<AnimationHandler>().PlayAnimation("SprintJumpToIdleAnim");
            }
        }
    }

    private void JumpForceOnMovingPlatformHandler()
    {
        if(BloquinhoBrancoController.instance != null)
        {
            if(BloquinhoBrancoController.instance.IsOnMovingPlatform()) 
            {
                if(lastMovingPlatform.IsMovingUp())
                    jumpForce = startJumpForce + 4f;
                else
                    jumpForce = startJumpForce;
            }
            else
            {
                jumpForce = startJumpForce;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        WayPointSystem wayPointObject = other.gameObject.GetComponent<WayPointSystem>();

        if(wayPointObject != null)
            lastMovingPlatform = wayPointObject;
    }

    public bool IsJumping
    {
        get{return isJumping;}
    }
}
