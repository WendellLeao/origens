using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    [Header("Check Obstacle")]
    [SerializeField] private float checkObstacleDistance;
    [SerializeField] private float originColliderOffSet;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private LayerMask whatIsGround;
    private BoxCollider2D boxCollider;
    private Vector2 origin;
    
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool IsGrounded()
    {
        float extraHeightText = 0.2f; //0.2f
        RaycastHit2D raycastHit;
        //Vector2 origin;

        // if(!GetComponent<SpriteRenderer>().flipX || !GetComponent<SpriteRenderer>().flipX && transform.localScale.x > 0f)
        //     origin = new Vector2(boxCollider.bounds.center.x + checkObstacleDistance, boxCollider.bounds.center.y);
        
        // else if(GetComponent<SpriteRenderer>().flipX || !GetComponent<SpriteRenderer>().flipX && transform.localScale.x < 0f)
        //     origin = new Vector2(boxCollider.bounds.center.x - checkObstacleDistance, boxCollider.bounds.center.y);  
        
        if(GetComponent<Slime>() != null)
        {
            if(!GetComponent<SpriteRenderer>().flipX)
                origin = new Vector2(boxCollider.bounds.center.x + checkObstacleDistance, boxCollider.bounds.center.y);
            else
                origin = new Vector2(boxCollider.bounds.center.x - checkObstacleDistance, boxCollider.bounds.center.y);      
        }      
        else
        {
            if(transform.localScale.x > 0f)
                origin = new Vector2(boxCollider.bounds.center.x + originColliderOffSet, boxCollider.bounds.center.y);
            else
                origin = new Vector2(boxCollider.bounds.center.x - originColliderOffSet, boxCollider.bounds.center.y);   
        }
        
        if(transform.localScale.y == 1f)
        {
            raycastHit = Physics2D.BoxCast(
                origin, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, whatIsGround
            );
        }
        else
        {
            raycastHit = Physics2D.BoxCast(
                origin, boxCollider.bounds.size, 0f, Vector2.up, extraHeightText, whatIsGround
            );
        }

        return raycastHit.collider != null;
    }
    
    public bool ThereIsObstacle_Right()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(
            boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right, checkObstacleDistance, whatIsObstacle
        );

        Color rayColor;
        if(raycastHitRight.collider != null)
            rayColor = Color.green;
        else    
            rayColor = Color.red;

        Debug.DrawRay(boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y), Vector2.right * (boxCollider.bounds.extents.x + checkObstacleDistance), rayColor);
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x + checkObstacleDistance, boxCollider.bounds.extents.y), Vector2.down * (boxCollider.bounds.size.y), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), Vector2.right * (boxCollider.bounds.extents.x + checkObstacleDistance), rayColor);

        return raycastHitRight.collider != null;
    }
    public bool ThereIsObstacle_Left()
    {
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(
            boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.left, checkObstacleDistance, whatIsObstacle
        );

        Color rayColor;
        if(raycastHitLeft.collider != null)
            rayColor = Color.green;
        else    
            rayColor = Color.red;

        Debug.DrawRay(boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y), Vector2.left * (boxCollider.bounds.extents.x + checkObstacleDistance), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x + checkObstacleDistance, boxCollider.bounds.extents.y), Vector2.up * (boxCollider.bounds.size.y), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), Vector2.left * (boxCollider.bounds.extents.x + checkObstacleDistance), rayColor);

        return raycastHitLeft.collider != null;
    }
}
