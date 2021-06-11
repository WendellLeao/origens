using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyController
{    
    [Header("Attack and Chase System")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToChase;
    [SerializeField] private float enemyForceAttack;
    private Transform bbPos;
    private float distancePlayer;
    private bool canDamagePlayer = true;
    
    [Header("Components and Enum")]
    private bool canChase = false;
    private bool canFlip;

    [Header("Hands")]
    [SerializeField] private GameObject ghostHand;
    [SerializeField] private GameObject ghostHand2;
    private Color startHandSpriteColor;

    [Header("Particle System")]
    [SerializeField] private GameObject ghostHandPS;
    [SerializeField] private GameObject ghostHand2PS;

    [Header("Others")]
    [SerializeField] private GameObject ghostCollider;

    protected override void Awake()
    {
        base.Awake();

        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();

        startHandSpriteColor = ghostHand.GetComponent<SpriteRenderer>().color;
    }

    protected override void Update()
    {
        base.Update();
        
        CheckPlayerDistance();

        if(canChase)    
            ChasePlayer();
        else
            GetComponent<AnimationHandler>().PlayAnimation("ghostIdleAnim");

        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            ghostHandPS.SetActive(false);
            ghostHand2PS.SetActive(false);
            ghostCollider.SetActive(false);
        }
        else
        {
            ghostCollider.SetActive(true);
        }
    }
    void FixedUpdate()
    {
        // if(canChase)    
        //     ChasePlayer();
        // else
        //     GetComponent<AnimationHandler>().PlayAnimation("ghostIdleAnim");
    }

    private void CheckPlayerDistance()
    {
        distancePlayer = Mathf.Abs(PlayerDistance(bbPos));

        canChase = distancePlayer <= distanceToChase && distancePlayer > 3f && Mathf.Abs(PlayerDistanceY(bbPos)) <= 7;        

        if(distancePlayer <= 2.5f)
        {
            if(transform.position.x > bbPos.position.x)
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(bbPos.transform.position.x + 3, bbPos.transform.position.y + 1.5f), moveSpeed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(bbPos.transform.position.x - 3, bbPos.transform.position.y + 1.5f), moveSpeed * Time.deltaTime);
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, bbPos.transform.position, moveSpeed * Time.deltaTime);

        if(distancePlayer >= 4f)
            GetComponent<AnimationHandler>().PlayAnimation("ghostChasingAnim");
        else
            GetComponent<AnimationHandler>().PlayAnimation("ghostAttackAnim");

        FlipHandler();
    }

    private void FlipHandler()
    {
        if(transform.position.x > bbPos.position.x && transform.localScale.x > 0
        || transform.position.x < bbPos.position.x && transform.localScale.x < 0)
        {
            Flip();
        }
    }

    protected override void EnemyDamage()
    {
        base.EnemyDamage();

        StartCoroutine(EnemyDamageAnim_Hand());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageHandler damageable = other.GetComponent<DamageHandler>();

        if(damageable != null)
        {
            if(canDamagePlayer)
            {
                bbPos.GetComponent<DamageHandler>().DamagePlayerPerSeconds(enemyForceAttack);
                canDamagePlayer = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        DamageHandler damageable = other.GetComponent<DamageHandler>();

        if(damageable != null)
        {
            bbPos.GetComponent<DamageHandler>().UnsubscribeDamagePlayerAmount(enemyForceAttack);
            
            canDamagePlayer = true;
        }
    }
    IEnumerator EnemyDamageAnim_Hand()
    {
        ghostHand.GetComponent<SpriteRenderer>().color = Color.red;
        ghostHand2.GetComponent<SpriteRenderer>().color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        ghostHand.GetComponent<SpriteRenderer>().color = startHandSpriteColor;
        ghostHand2.GetComponent<SpriteRenderer>().color = startHandSpriteColor;
    }
}
