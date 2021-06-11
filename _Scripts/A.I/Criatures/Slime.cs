using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyController
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject slimeCollider;
    [SerializeField] private ParticleSystem slimePS;
    private ObstacleChecker obstacleChecker;
    private bool canMove = true;
    private bool canFlip = true;
    private float timerToFlip;
    private bool canPlayParticleSystem = true;

    protected override void Awake()
    {
        base.Awake();

        obstacleChecker = GetComponent<ObstacleChecker>();

        if(transform.localScale.y < 0)
            healthBarPosY = healthBarPosY * -1;
    }
    protected override void Start()
    {
        base.Start();
        
        if(GetComponent<SpriteRenderer>().flipX)
            moveSpeed *= -1;
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
            TimerToFlipAgainHandler();
        }

        if(canMove && healthSystem.GetHealth() > 0f)
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0f));
    }
    void FixedUpdate()
    {
        // if(canMove && healthSystem.GetHealth() > 0f)
        //     transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0f));
    }
    
    private void ObstacleHandler()
    {
        if(obstacleChecker.IsGrounded())
        {
            if(obstacleChecker.ThereIsObstacle_Right() && obstacleChecker.ThereIsObstacle_Left())
                canMove = false;
            else
                canMove = true;
            
            if(obstacleChecker.ThereIsObstacle_Right() && !GetComponent<SpriteRenderer>().flipX && !obstacleChecker.ThereIsObstacle_Left()
            || obstacleChecker.ThereIsObstacle_Left() && GetComponent<SpriteRenderer>().flipX && !obstacleChecker.ThereIsObstacle_Right())
                Flip();
        }
        else
        {
            if(obstacleChecker.ThereIsObstacle_Right() || obstacleChecker.ThereIsObstacle_Left())
            {
                canMove = false;
            }
            else
            {
                canMove = true;

                Flip();
            }
        }
    }
    protected override void Flip()
    {
        if(canFlip && timerToFlip <= 0f)
        {
            timerToFlip = 0.5f;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            moveSpeed *= -1;
            
            canFlip = false;
        }
        else if(canFlip && timerToFlip >= 0f)
        {
            canFlip = false;
            canMove = false;
        }
    }

    private void TimerToFlipAgainHandler()
    {
        if(timerToFlip <= 0f)
        {
            canFlip = true;
            timerToFlip = 0f;
        }
        else
        {
            timerToFlip -= Time.deltaTime;
        }
    }

    IEnumerator TimeToDestroy()
    {
        //GetComponent<SpriteRenderer>().color = Color.red;

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

            if(BloquinhoPretoController.instance != null)   
                BloquinhoPretoController.instance.transform.parent = this.transform;

            if (this.transform.localScale.y < 0f)
                bb.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        BloquinhoBrancoController bb = other.GetComponent<BloquinhoBrancoController>();
        
        if(bb != null)
        {
            if(!bb.IsBeingGrabbed || !bb.IsGrounded())
            {
                if(BloquinhoPretoController.instance != null)   
                    BloquinhoPretoController.instance.transform.parent = null;

                bb.IsBeingGrabbed = false;
            }

            // if(this.transform.localScale.y < 0f)
            //     bb.transform.parent = null;
        }
    }
}
