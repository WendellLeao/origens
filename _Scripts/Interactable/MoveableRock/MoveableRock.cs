using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableRock : DragAndDrop, IDestroyable
{
    [Header("Others")]
    [SerializeField] private ParticleSystem spawnDustParticleSystem;
    [SerializeField] private Color heldColor;
    // [SerializeField] private GameObject outlineSpriteObj;
    [SerializeField] private BoxCollider2D colliderMovableRock;
    [SerializeField] private LayerMask layerPlayer, whatIsGround;
    private bool canPlayParticleSystem = true;
    private bool playerIsObstacle = false;
    private float startMass;
    private Color startColor;

    private bool canPlaySound = true;

    void Start()
    {
        startMass = body.mass;
        startColor = GetComponent<SpriteRenderer>().color;
        
        spawnDustParticleSystem.Play();    
    }

    protected override void Update()
    {
        base.Update();

        CheckIfPlayerIsObstacle();

        #region Sound (Commented)
        // if(isBeingPressed && IsGrounded())
        // {
        //     //AudioHandler.instance.Play("MovableRockFX");
        // }

        // if(body.velocity.x != 0f && IsGrounded()) // if(Mathf.Abs(body.velocity.x) > 0.0000001f)
        //     AudioHandler.instance.Play("MovableRockFX");
        // else
        //     AudioHandler.instance.StopPlaying("MovableRockFX");

        // if (canPlaySound)
        // {
        //     AudioHandler.instance.Play("MovableRockFX");
        //     //canPlaySound = false;
        // }
        #endregion

        #region Color (Commented)
        // if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        // {
        //     GetComponent<SpriteRenderer>().color = startColor;
        //     outlineSpriteObj.SetActive(false);
        // }

        // if(isBeingPressed)
        // {
        //     GetComponent<SpriteRenderer>().color = heldColor;
        //     outlineSpriteObj.SetActive(false);
        // }
        // else
        // {
        //     GetComponent<SpriteRenderer>().color = startColor;
        // }

        // if(Telecinese.instance != null)
        // {
        //     if(playerIsObstacle)
        //     {
        //         if(isBeingPressed)
        //             Telecinese.instance.GetComponent<TrailRenderer>().startColor = Color.red;
        //         else
        //             Telecinese.instance.GetComponent<TrailRenderer>().startColor = Telecinese.instance.StartColor;
        //     }
        //     else
        //     {
        //         Telecinese.instance.GetComponent<TrailRenderer>().startColor = Telecinese.instance.StartColor;
        //     }
        // }

        //Debug.Log(IsGrounded());
        #endregion
    }    

    private void CheckIfPlayerIsObstacle()
    {
        if (IsTouchingPlayer_Left())
        {
            if (MouseDirection() < 0f)
                isBeingHeld = false;
        }

        if (IsTouchingPlayer_Right())
        {
            if (MouseDirection() > 0f)
                isBeingHeld = false;
        }

        if (IsTouchingPlayer_Up())
            isBeingHeld = false;
    }
    
    private float MouseDirection()
    {
        return Input.GetAxis("Mouse X");
    }

    private bool IsTouchingPlayer_Up()
    {
        float extraHeightText = 0.06f; //0.2f

        RaycastHit2D raycastHitUp = Physics2D.BoxCast(
            colliderMovableRock.bounds.center, colliderMovableRock.bounds.size, 0f, Vector2.up, extraHeightText, layerPlayer
        );

        return raycastHitUp.collider != null;
    }
    private bool IsTouchingPlayer_Right()
    {
        float extraHeightText = 0.06f; //0.06f

        RaycastHit2D raycastHitRight = Physics2D.BoxCast(
            colliderMovableRock.bounds.center, colliderMovableRock.bounds.size, 0f, Vector2.right, extraHeightText, layerPlayer
        );

        return raycastHitRight.collider != null;
    }
    private bool IsTouchingPlayer_Left()
    {
        float extraHeightText = 0.06f; //0.2f //0.06

        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(
            colliderMovableRock.bounds.center, colliderMovableRock.bounds.size, 0f, Vector2.left, extraHeightText, layerPlayer
        );

        return raycastHitLeft.collider != null;
    }

    private bool IsGrounded()
    {
        float extraHeightText = 0.2f; //0.2f
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(colliderMovableRock.bounds.center, colliderMovableRock.bounds.size, 0f, Vector2.down, extraHeightText, whatIsGround);
        Color rayColor;
        
        if(raycastHit.collider != null)
            rayColor = Color.green;
        else    
            rayColor = Color.red;

        Debug.DrawRay(colliderMovableRock.bounds.center + new Vector3(colliderMovableRock.bounds.extents.x, 0), Vector2.down * (colliderMovableRock.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(colliderMovableRock.bounds.center - new Vector3(colliderMovableRock.bounds.extents.x, 0), Vector2.down * (colliderMovableRock.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(colliderMovableRock.bounds.center - new Vector3(colliderMovableRock.bounds.extents.x, colliderMovableRock.bounds.extents.y + extraHeightText), Vector2.right * (colliderMovableRock.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }
    
    public void DestroyObject()
    {
        StartCoroutine(DestroyObjectTimer());
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(other.gameObject.tag == "Lava" || other.gameObject.tag == "DeadZone")
            StartCoroutine(DestroyObjectTimer());
    }

    public IEnumerator DestroyObjectTimer()
    {
        yield return new WaitForSeconds(0.6f);

        if(canPlayParticleSystem)
        {
            AudioHandler.instance.Play("DestructionFX");

            spawnDustParticleSystem.transform.position = transform.position;
            spawnDustParticleSystem.Play();    
            
            canPlayParticleSystem = false;
        }
        
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        colliderMovableRock.enabled = false;

        yield return new WaitForSeconds(spawnDustParticleSystem.main.startLifetime.constantMax);
        Destroy(this.gameObject);
    }
}
