using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Transform endPosTransform;

    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 nextPos;

    //ADDED
    private Rigidbody2D player;
    private Vector3 moveDelta;

    void Start()
    {
        startPos = transform.localPosition;
        endPos = endPosTransform.localPosition;
        nextPos = endPos;
    }

    void Update()
    {
        SetMovement();
        SetMoveDirection();
    }

    //Move the player by the change in movement of the platform using the player's rigidbody instead of parenting
    void LateUpdate()
    {
        if (player)
        {
            Vector2 playerBody = player.position;
            player.transform.position = new Vector3(playerBody.x, playerBody.y) + moveDelta;
        }
    }

    void SetMovement()
    {
        //Calculate desired position
        Vector2 desiredPosition = Vector2.MoveTowards(transform.localPosition, nextPos, speed * Time.deltaTime);
        
        //Use that position to figure out the change in position of the platform
        moveDelta = new Vector3(desiredPosition.x, desiredPosition.y, 0f)- transform.localPosition;
        //moveDelta = new Vector3(desiredPosition.x, desiredPosition.y, 0f) - transform.position;

        //Apply the new position
        transform.localPosition = desiredPosition;
    }

    void SetMoveDirection()
    {
        if (Vector2.Distance(transform.localPosition, nextPos) <= 0.1)
        {
            if (nextPos == endPos)
            {
                nextPos = startPos;
            }
            else
            {
                nextPos = endPos;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Instead of parenting, grab a reference to the player's rigidbody
        if (other.gameObject.CompareTag("Player"))
            player = other.gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //Remove reference
        if (other.gameObject.CompareTag("Player"))
            player = null;
    }
}
