using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : EnemyController
{    
    [Header("Attack and Chase System")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToChase;
    [SerializeField] private float enemyForceAttack;
    private Transform bbPos;
    
    [Header("Components and Enum")]
    private bool canChase = false;
    private bool canFlip;

    protected override void Awake()
    {
        base.Awake();

        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }

    protected override void Update()
    {
        base.Update();

        CheckPlayerDistance();

        if(canChase)    
            ChasePlayer();
    }
    void FixedUpdate()
    {
        // if(canChase)    
        //     ChasePlayer();
    }

    private void CheckPlayerDistance()
    {
        float distancePlayer = Mathf.Abs(PlayerDistance(bbPos));
        
        canChase = distancePlayer <= distanceToChase && distancePlayer > 1f && Mathf.Abs(PlayerDistanceY(bbPos)) <= 5;        
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, bbPos.transform.position, moveSpeed * Time.deltaTime);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
        if(damageablePlayer != null)
        {
            AudioHandler.instance.Play("PlayerBeHitedFX");
            //damageablePlayer.SetDamagePerSecond(enemyForceAttack);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
        //if(damageablePlayer != null)
            //damageablePlayer.ResetDamagePerSecond(enemyForceAttack);
    }

    public bool CanChase
    {
        get{return canChase;}
    }
}
