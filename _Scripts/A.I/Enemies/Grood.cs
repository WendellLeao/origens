using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grood : EnemyController
{
    public enum State { Patrolling, Chasing, Attacking }

    [Header("Patrolling")]
    //[SerializeField] private float moveSpeed;
    private ObstacleChecker obstacleChecker;

    [Header("Chase")]
    [SerializeField] private float distanceToChase;

    [Header("Attack")]
    [SerializeField] private float enemyForceAttack;
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float timeBtwAttack;
    [SerializeField] private BoxCollider2D damageHand;
    private float startTimeBtwAttack;
    private float timeToStopAttack;
    private float distancePlayer;

    [Header("Others")]
    [SerializeField] private GameObject colliderGrood;
    [SerializeField] private GameObject nucleoObj;
    AnimationHandler animationHandler;
    EnemyPatrol enemyPatrol;
    private Transform bbPos;
    private Vector2 newPos;
    private State state;
    private bool ignorePlayer = false;
    private bool isAttacking = false;

    [Header("Death Anim")]
    [SerializeField] private ParticleSystem groodPS;
    [SerializeField] private GameObject groodCollision;
    [SerializeField] private GameObject groodCollisionGround;
    [SerializeField] private GameObject detailsObj;
    private bool canPlayParticleSystem = true;
    private bool canMove;
    private bool canFlip = true;
    private float timerToFlip;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D body;
    private bool isFalling = false;

    protected override void Awake()
    {
        base.Awake();

        body = GetComponent<Rigidbody2D>();

        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
        
        animationHandler = GetComponent<AnimationHandler>();
        enemyPatrol = GetComponent<EnemyPatrol>();

        //damageHand = GetComponent<BoxCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        obstacleChecker = GetComponent<ObstacleChecker>();

        state = State.Patrolling;
    }
    protected override void Start()
    {
        base.Start();
        
        startTimeBtwAttack = timeBtwAttack;
        //timeToStopAttack = startTimeBtwAttack;
        timeToStopAttack = 2.4f;

        timeBtwAttack = 0;

        // if(transform.localScale.x > 0f)
        //     moveSpeed = +moveSpeed;
        // else
        //     moveSpeed = -moveSpeed;
        ignorePlayer = false;
        canMove = true;
    }
    void FixedUpdate()
    {
        if(state == State.Chasing && distancePlayer > 1 && canMove)
            enemyPatrol.body.MovePosition(newPos);
            //GetComponent<Rigidbody2D>().MovePosition(newPos);
    }
    protected override void Update()
    {
        //base.Update();

        if(Input.GetKeyDown(KeyCode.N)) 
            ignorePlayer = false;
        
        if(healthSystem.GetHealth() > 0f)
        {
            CheckPlayerDistance();

            switch(state)
            {
                case State.Patrolling: Patrolling(); break;
                case State.Attacking: Attacking(); break;
                case State.Chasing: Chasing(); break;
            }

            if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            {
                nucleoObj.SetActive(false);
                //colliderGrood.SetActive(false);
            }
            else
            {
                nucleoObj.SetActive(true);
                //colliderGrood.SetActive(true);
            }

            //if(state == State.Chasing && enemyPatrol.TheresWall() || state == State.Chasing && !enemyPatrol.TheresGround())
            if(state == State.Chasing && obstacleChecker.ThereIsObstacle_Right() || state == State.Chasing && 
            obstacleChecker.ThereIsObstacle_Left() || state == State.Chasing && !obstacleChecker.IsGrounded())
                ignorePlayer =  true;

            if(ignorePlayer && state != State.Patrolling)
                state = State.Patrolling;

            // if(ignorePlayer)
            //     StartCoroutine(TimeToDetect());

            if(!canMove)
                animationHandler.PlayAnimation("GroodIdleAnim");
        }
        else
        {
            if(healthBar != null)
                healthBar.DestroyHealthBar();

            damageHand.enabled = false;
            
            nucleoObj.SetActive(false);
            colliderGrood.SetActive(false);

            groodCollision.SetActive(false);
            groodCollisionGround.SetActive(false);

            canMove = false;
            // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemyPatrol.enabled = false;
            enemyPatrol.body.velocity = Vector2.zero;
            
            animationHandler.PlayAnimation("GroodDeathAnim");
            //StartCoroutine(Break());
        }
        
        // if(!enemyPatrol.TheresGround())
        // {
        //     if(GetComponent<Rigidbody2D>() == null)
        //         gameObject.AddComponent<Rigidbody2D>();
        // }
    }

    private void Patrolling()
    {
        if(canMove)
        {
            //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animationHandler.PlayAnimation("GroodWalkAnim");
        }

        TimerToFlipAgainHandler();
        ObstacleHandler();

        damageHand.enabled = false;

        isAttacking = false;

        timeBtwAttack = 0;
    }
    private void Chasing()
    {
        if(!obstacleChecker.ThereIsObstacle_Left() && !obstacleChecker.ThereIsObstacle_Right() && obstacleChecker.IsGrounded() && !ignorePlayer)
        {    
            //canMove = true;

            damageHand.enabled = false;

            newPos = Vector2.MoveTowards(transform.position, bbPos.transform.position, Time.deltaTime * 2.5f);

            if(canMove)
                animationHandler.PlayAnimation("GroodWalkAnim");

            isAttacking = false;
            
            timeBtwAttack = 0;

            FlipHandler();
        }
        else
        {
            ignorePlayer = true;
            state = State.Patrolling;
        }
    }
    private void Attacking()
    {
        if(!obstacleChecker.ThereIsObstacle_Left() && !obstacleChecker.ThereIsObstacle_Right() && obstacleChecker.IsGrounded() && !ignorePlayer)
        {
            //canMove = false;
            timeBtwAttack -= Time.deltaTime;
            
            if(timeBtwAttack <= 0f)
            {
                //canMove = false;
                isAttacking = true;

                animationHandler.PlayAnimation("GroodAttackAnim");
                damageHand.enabled = true;

                timeToStopAttack -= Time.deltaTime;

                if(timeToStopAttack <= 0f)
                {
                    timeBtwAttack = startTimeBtwAttack;
                    //timeToStopAttack = startTimeBtwAttack;
                    timeToStopAttack = 2.4f;
                }
            }
            else
            {
                isAttacking = false;
                //animationHandler.PlayAnimation("GroodIdleAnim");
                animationHandler.PlayAnimation("GroodReadyToAttackAnim");
                damageHand.enabled = false;
            }

            FlipHandler();
        }
        else
        {
            ignorePlayer = true;
            state = State.Patrolling;
        }
    }

    private void CheckPlayerDistance()
    {
        float distancePlayerY = Mathf.Abs(PlayerDistanceY(bbPos));

        if(!isAttacking)
        {
            if (distancePlayerY <= 3f)
            {
                distancePlayer = Mathf.Abs(PlayerDistance(bbPos));

                if (distancePlayer <= Mathf.Abs(distanceToChase) && distancePlayer >= Mathf.Abs(distanceToAttack) && !ignorePlayer)
                {
                    state = State.Chasing;
                }
                else if (distancePlayer <= Mathf.Abs(distanceToAttack))
                {
                    ignorePlayer = false;
                    state = State.Attacking;
                }
                else
                {
                    state = State.Patrolling;
                }
            }
            else
            {
                state = State.Patrolling;
            }
        }
        else
        {
            state = State.Attacking;
        }
    }

    private void ObstacleHandler()
    {
        if(obstacleChecker.IsGrounded())
        {
            if(obstacleChecker.ThereIsObstacle_Right() && obstacleChecker.ThereIsObstacle_Left())
                canMove = false;
            else
                canMove = true;

            if(obstacleChecker.ThereIsObstacle_Right() && transform.localScale.x > 0f && !obstacleChecker.ThereIsObstacle_Left()
            || obstacleChecker.ThereIsObstacle_Left() && transform.localScale.x < 0f && !obstacleChecker.ThereIsObstacle_Right())
            {
                GroodFlip();
            }

            //body.constraints = RigidbodyConstraints2D.FreezePositionY;
            //body.constraints = RigidbodyConstraints2D.FreezeRotation;

            //body.isKinematic = false;
            //body.gravityScale = 1.5f;
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

                GroodFlip();
            }

            //FallGapHandler();
        }

        //Debug.Log(isFalling);
    }

    private void FlipHandler()
    {
        if(state != State.Patrolling)//canFlip
        {
            if(transform.position.x > bbPos.position.x && transform.localScale.x > 0
            || transform.position.x < bbPos.position.x && transform.localScale.x < 0)
            {
                Flip();
                
                float moveSpeed = enemyPatrol.GetMoveSpeed();
                moveSpeed *= -1;

                enemyPatrol.SetMoveSpeed(moveSpeed);
            }
        }
    }
    
    public void ShakeCamera()
    {
        CinemachineShake.instance.ShakeCamera(8f, 0.2f);
    }

    private void GroodFlip()
    {
        if(canFlip && timerToFlip <= 0f)// && timerToFlip <= 0f
        {
            timerToFlip = 0.5f;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;
            
            transform.localScale = theScale;

            FlipHealthBar();
            
            float moveSpeed = enemyPatrol.GetMoveSpeed();
            moveSpeed *= -1;
            enemyPatrol.SetMoveSpeed(moveSpeed);
            
            canFlip = false;
        }
        else if(canFlip && timerToFlip >= 0f)
        {
            canFlip = false;
            canMove = false;

            isFalling = true;
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

    private void FallGapHandler()
    {
        body.constraints = RigidbodyConstraints2D.None;

        body.isKinematic = false;
        body.gravityScale = 1.5f;

        body.velocity = new Vector2(0f, body.velocity.y);

        animationHandler.PlayAnimation("GroodIdleAnim");
    }

    // IEnumerator TimeToDetect()
    // {
    //     yield return new WaitForSeconds(1f);
    //     ignorePlayer = false;
    // }

    public IEnumerator Break()
    {
        if(canPlayParticleSystem)
        {
            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            groodPS.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;

        detailsObj.SetActive(false);

        yield return new WaitForSeconds(groodPS.main.startLifetime.constantMax);
        Destroy(this.gameObject);
    }
    
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
    //     if(damageablePlayer != null)
    //     {
    //         AudioHandler.instance.Play("PlayerBeHitedFX");
    //         bbPos.GetComponent<DamageHandler>().DamagePlayerPerUnit(enemyForceAttack);
    //     }
    // }

    public State GetState()
    {
        return state;
    }

    public bool CanMove
    {
        get{return this.canMove;}
    }
}
