using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWall : EnemyController
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject slimeCollider;
    [SerializeField] private ParticleSystem slimePS;

    [Header("Check Obstacle")]
    [SerializeField] private float checkObstacleDistance;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private LayerMask whatIsGround;
    private BoxCollider2D boxCollider;
    private bool canMove = true;
    private bool canPlayParticleSystem = true;

    protected override void Awake()
    {
        base.Awake();

        boxCollider = GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        base.Start();
        
        if(!GetComponent<SpriteRenderer>().flipY)
            moveSpeed *= -1;
    }
    void FixedUpdate()
    {
        // if(canMove && healthSystem.GetHealth() > 0f)
        //     transform.Translate(new Vector2(0f, moveSpeed * Time.deltaTime));
    }
    protected override void Update()
    {
        //base.Update();

        if(healthSystem.GetHealth() <= 0f)
        {
            StartCoroutine(TimeToDestroy());

            if(BloquinhoBrancoController.instance.GetComponent<Transform>().parent == this.transform)
            {
                BloquinhoBrancoController.instance.GetComponent<Transform>().parent = null;
                BloquinhoBrancoController.instance.IsBeingGrabbed = false;
            }
        }
        else
        {
            ObstacleHandler();
        }

        if(canMove && healthSystem.GetHealth() > 0f)
            transform.Translate(new Vector2(0f, moveSpeed * Time.deltaTime));
    }

    private bool ThereIsWall()
    {
        float extraHeightText = 0.5f; //0.2f
        RaycastHit2D raycastHit;
        Vector2 origin;

        if(!GetComponent<SpriteRenderer>().flipY)
            origin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - checkObstacleDistance);
        else
            origin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y + checkObstacleDistance);

        if(transform.localScale.x == 1f)
        {
            raycastHit = Physics2D.BoxCast(
                origin, boxCollider.bounds.size, 0f, Vector2.left, extraHeightText, whatIsGround
            );
        }
        else
        {
            raycastHit = Physics2D.BoxCast(
                origin, boxCollider.bounds.size, 0f, Vector2.right, extraHeightText, whatIsGround
            );
        }

        return raycastHit.collider != null;
    }
    
    private bool ThereIsObstacle_Up()
    {
        RaycastHit2D raycastHitUp = Physics2D.BoxCast(
            boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, checkObstacleDistance, whatIsObstacle
        );

        return raycastHitUp.collider != null;
    }
    private bool ThereIsObstacle_Down()
    {
        RaycastHit2D raycastHitDown = Physics2D.BoxCast(
            boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, checkObstacleDistance, whatIsObstacle
        );

        return raycastHitDown.collider != null;
    }

    private void ObstacleHandler()
    {
        if(ThereIsWall())
        {
            if (ThereIsObstacle_Up() && ThereIsObstacle_Down())
                canMove = false;
            else
                canMove = true;

            if (ThereIsObstacle_Up() && GetComponent<SpriteRenderer>().flipY && !ThereIsObstacle_Down()
            || ThereIsObstacle_Down() && !GetComponent<SpriteRenderer>().flipY && !ThereIsObstacle_Up())
                Flip();
        }
        else
        {
            Flip();
        }
    }

    protected override void Flip()
    {
        GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
        moveSpeed *= -1;
    }

    IEnumerator TimeToDestroy()
    {
        //GetComponent<SpriteRenderer>().color = Color.red;

        GetComponent<EdgeCollider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        slimeCollider.SetActive(false);
        
        if(healthBar != null)
            healthBar.DestroyHealthBar();

        GetComponent<AnimationHandler>().PlayAnimation("SlimeDeathAnim");

        yield return new WaitForSeconds(0.65f);//0.5f

        if(canPlayParticleSystem)
        {
            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            slimePS.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(slimePS.main.startLifetime.constantMax);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BloquinhoBrancoController bb = other.GetComponent<BloquinhoBrancoController>();
        
        if(bb != null)
        {
            bb.transform.parent = this.transform;
            bb.IsBeingGrabbed = true;
            
            bb.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        BloquinhoBrancoController bb = other.GetComponent<BloquinhoBrancoController>();
        
        if(bb != null)
            bb.IsBeingGrabbed = false;
    }
}
