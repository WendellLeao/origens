using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalObstacleChecker : MonoBehaviour
{
    //[Header("Options")]
    //[SerializeField] private bool checkInX;
    //[SerializeField] private bool checkInY;
    
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private float horizontalDistance;
    [SerializeField] private float verticalDistance;
    private WayPointSystem wayPoint;

    void Awake()
    {
        wayPoint = GetComponent<WayPointSystem>();
    }

    void Update()
    {
        if(TheresObstacle())
            wayPoint.enabled = false;
        else
            wayPoint.enabled = true;

        /*if(checkHorizontalDistance)
        {
            if(ThereIsObstacleInHorizontal())
                wayPoint.enabled = false;
            else
                wayPoint.enabled = true;
        }

        if(checkVerticalDistance)
        {
            if(ThereIsObstacleInVertical())
                wayPoint.enabled = false;
            else
                wayPoint.enabled = true;
        }*/
    }

    private bool TheresObstacle()
    {
        //Horizontal
        RaycastHit2D raycastRight = Physics2D.Raycast(transform.position, Vector2.right, horizontalDistance, whatIsObstacle);
        RaycastHit2D raycastLeft = Physics2D.Raycast(transform.position, Vector2.left, horizontalDistance, whatIsObstacle);
        
        //Vertical
        RaycastHit2D raycastDown = Physics2D.Raycast(transform.position, Vector2.down, horizontalDistance, whatIsObstacle);
        RaycastHit2D raycastUp = Physics2D.Raycast(transform.position, Vector2.up, verticalDistance, whatIsObstacle);

        Color raycolor;

        if(raycastRight.collider == null || raycastLeft.collider == null || raycastUp.collider == null || raycastDown.collider == null)
            raycolor = Color.green;
        else
            raycolor = Color.red;
        
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + horizontalDistance, transform.position.y), raycolor);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x - horizontalDistance, transform.position.y), raycolor);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + verticalDistance), raycolor);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - horizontalDistance), raycolor);
        
        return raycastRight.collider != null || raycastLeft.collider != null || raycastUp.collider != null || raycastDown.collider != null;
    }

    private bool ThereIsObstacleInHorizontal()
    {
        RaycastHit2D raycastRight = Physics2D.Raycast(transform.position, Vector2.right, horizontalDistance, whatIsObstacle);
        RaycastHit2D raycastLeft = Physics2D.Raycast(transform.position, Vector2.left, horizontalDistance, whatIsObstacle);
        Color raycolor;

        if(raycastRight.collider == null || raycastLeft.collider == null)
            raycolor = Color.green;
        else
            raycolor = Color.red;

        Debug.DrawLine(transform.position, new Vector2(transform.position.x + horizontalDistance, transform.position.y), raycolor);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x - horizontalDistance, transform.position.y), raycolor);

        return raycastRight.collider != null || raycastLeft.collider != null;
    }
    private bool ThereIsObstacleInVertical()
    {
        RaycastHit2D raycastDown = Physics2D.Raycast(transform.position, Vector2.down, horizontalDistance, whatIsObstacle);
        RaycastHit2D raycastUp = Physics2D.Raycast(transform.position, Vector2.up, verticalDistance, whatIsObstacle);
        Color raycolor;

        if(raycastUp.collider == null || raycastDown.collider == null)
            raycolor = Color.green;
        else
            raycolor = Color.red;

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + verticalDistance), raycolor);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - horizontalDistance), raycolor);

        return raycastUp.collider != null || raycastDown.collider != null;            
    }
}
