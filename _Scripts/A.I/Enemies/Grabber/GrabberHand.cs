using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberHand : MonoBehaviour
{
    [Header("Grabber")]
    [SerializeField] private Grabber grabber;
    [SerializeField] private float grabberForceAttack;
    [SerializeField] private float moveSpeed;

    [Header("Hands Colors")]
    [SerializeField] private SpriteRenderer handSpriteRenderer;
    [SerializeField] private SpriteRenderer overHandSpriteRenderer;
    [SerializeField] private SpriteRenderer grabberSpriteRenderer;
    private Color startHandSpriteColor;
    private Transform bbPos;
    private bool canDamagePlayer = true;

    void Awake()
    {
        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }
    void Start()
    {
        startHandSpriteColor = handSpriteRenderer.color;
    }
    void OnEnable()
    {
        grabber.onGrabberHited += OnGrabberHited_ChangeSpriteColorHandler;
    }

    void Update()//FixedUpdate()
    {
        MovementHandler();
    }

    private void OnGrabberHited_ChangeSpriteColorHandler()
    {
        StartCoroutine(EnemyDamageAnim());
    }

    private void MovementHandler()
    {
        if(grabber.CanChase)
        {
            FlipHandler();

            if (HandsAreBehindTheBody() && Mathf.Abs(BodyDistance()) > 0.1f)
                transform.position = Vector2.MoveTowards(transform.position, grabber.transform.position, moveSpeed * Time.deltaTime * 2f);
            else
                transform.position = Vector2.MoveTowards(transform.position, bbPos.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if(transform.position != grabber.transform.position)
            {
                Vector2 newPos;

                if(grabber.transform.position.x < transform.position.x)
                    newPos = new Vector2(grabber.transform.position.x + 1f, grabber.transform.position.y);
                else
                    newPos = new Vector2(grabber.transform.position.x - 1f, grabber.transform.position.y);
                
                transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime * 2f);
            }
        }
    }

    private bool HandsAreBehindTheBody()
    {
        return transform.position.x < grabber.transform.position.x && bbPos.transform.position.x > grabber.transform.position.x
            || transform.position.x > grabber.transform.position.x && bbPos.transform.position.x < grabber.transform.position.x;
    }

    private float BodyDistance()
    {
        return transform.position.x - grabber.transform.position.x;
    }

    private void FlipHandler()
    {
        if(transform.position.x > bbPos.position.x && transform.localScale.x > 0
        || transform.position.x < bbPos.position.x && transform.localScale.x < 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        Vector3 theScale = transform.localScale;

        theScale.x *= -1;

        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
        if(damageablePlayer != null)
        {
            if(canDamagePlayer)
            {
                //AudioHandler.instance.Play("PlayerBeHitedFX");
                damageablePlayer.DamagePlayerPerSeconds(grabberForceAttack);
                canDamagePlayer = false;
            }

            if(!damageablePlayer.GetComponent<BloquinhoBrancoController>().IsBeingGrabbed)
                damageablePlayer.GetComponent<BloquinhoBrancoController>().IsBeingGrabbed = true;

            // if(other.GetComponent<MovementWithKeyboard>() != null)
            // {
            //     if(other.GetComponent<MovementWithKeyboard>().IsOnPlatform())
            //         other.transform.parent = this.transform;
            // }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
        if(damageablePlayer != null)
        {
            damageablePlayer.UnsubscribeDamagePlayerAmount(grabberForceAttack);

            if(damageablePlayer.GetComponent<BloquinhoBrancoController>().IsBeingGrabbed)
                damageablePlayer.GetComponent<BloquinhoBrancoController>().IsBeingGrabbed = false;

            canDamagePlayer = true;

            // if(other.GetComponent<MovementWithKeyboard>() != null)
            // {
            //     if(other.GetComponent<MovementWithKeyboard>().IsOnPlatform())
            //         other.transform.parent = null;
            // }
        }
    }

    IEnumerator EnemyDamageAnim()
    {
        handSpriteRenderer.GetComponent<SpriteRenderer>().color = Color.red;
        overHandSpriteRenderer.GetComponent<SpriteRenderer>().color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        // handSpriteRenderer.GetComponent<SpriteRenderer>().color = startHandSpriteColor;
        // overHandSpriteRenderer.GetComponent<SpriteRenderer>().color = startHandSpriteColor;
        handSpriteRenderer.GetComponent<SpriteRenderer>().color = grabberSpriteRenderer.color;
        overHandSpriteRenderer.GetComponent<SpriteRenderer>().color = grabberSpriteRenderer.color;
    }

    void OnDisable()
    {
        grabber.onGrabberHited -= OnGrabberHited_ChangeSpriteColorHandler;
    }
}
