using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Check Obstacle")]
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private Transform checkObstaclePos;
    [SerializeField] private float checkObstacleDistance;
    [HideInInspector] public Rigidbody2D body;
    private bool theresGround, theresWall;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if(transform.localScale.x > 0f)
            moveSpeed = +moveSpeed;
        else
            moveSpeed = -moveSpeed;

    }
    
    void Update()
    {
        // CheckGround();
        // CheckWall();

        if(GetComponent<Grood>() != null)
        {
            if(GetComponent<Grood>().GetState() == Grood.State.Patrolling && GetComponent<Grood>().CanMove)
                MovementHandler(moveSpeed);
        }
        else
        {
            MovementHandler(moveSpeed);
        }
    }

    void FixedUpdate()
    {
        // if(GetComponent<Grood>() != null)
        // {
        //     if(GetComponent<Grood>().GetState() == Grood.State.Patrolling)
        //     {
        //         MovementHandler(moveSpeed);
        //     }
        // }
        // else
        // {
        //     MovementHandler(moveSpeed);
        // }
    }

    public void MovementHandler(float moveSpeed)
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void CheckGround()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(checkObstaclePos.position, Vector2.down, checkObstacleDistance);
        
        if(groundInfo.collider == false)
        {
            Flip();
            theresGround = false;
        }
        else
        {
            theresGround = true;
        }
    }
    private void CheckWall()
    {
        RaycastHit2D wallInfo = Physics2D.Raycast(checkObstaclePos.position, Vector2.right, checkObstacleDistance, whatIsObstacle);
        
        if(wallInfo.collider == true)
        {
            Flip();

            theresWall = true;
        }
        else
        {
            theresWall = false;
        }
    }

    void Flip()
    {
        // Vector3 theScale = transform.localScale;

        // theScale.x *= -1;
        // moveSpeed *= -1;
        
        // transform.localScale = theScale;

        // if(GetComponent<EnemyController>() != null)
        //     GetComponent<EnemyController>().EnemyFlip();
        
        // moveSpeed *= -1;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float SetMoveSpeed(float newMoveSpeed)
    {
        this.moveSpeed = newMoveSpeed;
        return moveSpeed;
    }
    public bool TheresGround()
    {
        return theresGround;
    }
    public bool TheresWall()
    {
        return theresWall;
    }
}
