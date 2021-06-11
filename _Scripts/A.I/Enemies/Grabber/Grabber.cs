using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : EnemyController
{    
    [Header("Attack and Chase System")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToChase;

    [Header("Objects")]
    [SerializeField] private GameObject grabberHandObj;
    [SerializeField] private GameObject grabberTrigger;
    [SerializeField] private ParticleSystem grabberPS;
    private float distancePlayer;
    private Transform bbPos;
    private bool canChase = false;
    private bool canGrabOnPlatform = false;
    public event Action onGrabberHited;
    private bool canPlayParticleSystem = true;

    protected override void Awake()
    {
        base.Awake();

        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }

    protected override void Update()
    {
        if(healthSystem.GetHealth() <= 0f)
        {
            StartCoroutine(TimeToDestroy());
        }
        // else
        // {
        //     // if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        //     //     grabberTrigger.SetActive(false);
        //     // else
        //     //     grabberTrigger.SetActive(true);

        //     //FloatingAnimHandler();
        // }

        CheckPlayerDistance();

        if(healthSystem.GetHealth() > 0f)
        {
            if(canChase)    
                ChasePlayer();
            // else
            //     FloatingAnimHandler();
        }
    }

    void FixedUpdate()
    {
        // if(healthSystem.GetHealth() > 0f)
        // {
        //     if(canChase)    
        //         ChasePlayer();
        //     else
        //         FloatingAnimHandler();
        // }

        if(healthSystem.GetHealth() > 0f)
        {
            if(!canChase)    
                FloatingAnimHandler();
        }
    }

    private void CheckPlayerDistance()
    {
        distancePlayer = Mathf.Abs(PlayerDistance(bbPos));

        canChase = distancePlayer <= distanceToChase && Mathf.Abs(PlayerDistanceY(bbPos)) <= 7 && healthSystem.GetHealth() > 0;//5        
    }

    private void ChasePlayer()
    {
        if(distancePlayer > 1f)
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

    protected override void EnemyDamage()
    {
        base.EnemyDamage();

        onGrabberHited?.Invoke();
    }

    private void FloatingAnimHandler()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(
            transform.position.y - 0.02f, transform.position.y + 0.02f, Mathf.PingPong(Time.time * 0.65f, 1))//transform.position.y - 0.02f, transform.position.y + 0.02f,
        );
    }

    public bool CanChase
    {
        get{return canChase;}
    }

    IEnumerator TimeToDestroy()
    {
        grabberHandObj.SetActive(false);
        grabberTrigger.SetActive(false);
        
        if(healthBar != null)
            healthBar.DestroyHealthBar();

        GetComponent<AnimationHandler>().PlayAnimation("GrabberDeathAnim");

        yield return new WaitForSeconds(0.3f);//0.3f

        if(canPlayParticleSystem)
        {
            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            grabberPS.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(grabberPS.main.startLifetime.constantMax);
        Destroy(transform.parent.gameObject);
    }
}
