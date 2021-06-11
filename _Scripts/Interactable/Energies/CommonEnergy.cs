using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnergy : MonoBehaviour
{
    [SerializeField] private ParticleSystem commonEnergyPS;
    private bool canCollide = true;
    private bool canGoToBB = false;
    private float speedToGo;
    private Transform bbPos;
    private MovementWithKeyboard bbMovement;
    private float newSpeedToGo;

    void Awake()
    {
        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
        bbMovement = FindObjectOfType<MovementWithKeyboard>();
    }

    void Start()
    {
        this.transform.parent = null;
        MovementWithKeyboard.onPlayerFlip += Flip;

        newSpeedToGo = speedToGo * 1.5f; //1.3f
    }

    void Update()
    {
        if(transform.position.x == bbPos.transform.position.x && canCollide)
            GetCollected();

        float distanceX = Mathf.Abs(transform.position.x - bbPos.transform.position.x);
        //float distanceY = Mathf.Abs(transform.position.y - bbPos.transform.position.y);

        if(distanceX >= 5f) //|| distanceY >= 6f
            this.transform.parent = bbPos.transform;

        if(this.transform.parent == bbPos.transform)
        {
            if(!bbMovement.IsFacingRight() && transform.position.x < bbPos.transform.position.x
            || bbMovement.IsFacingRight() && transform.position.x > bbPos.transform.position.x)
            {
                speedToGo = newSpeedToGo;
            }
        }

        if(canGoToBB)
            transform.position = Vector2.MoveTowards(transform.position, bbPos.transform.position, speedToGo * Time.deltaTime);
    }
    
    void FixedUpdate()
    {
        // if(canGoToBB)
        //     transform.position = Vector2.MoveTowards(transform.position, bbPos.transform.position, speedToGo * Time.deltaTime);
    }
    
    private void GetCollected()
    {
        StaminaSystem.instance.AddStamina();
        AudioHandler.instance.Play("CommonEnergyFX");
        StartCoroutine(DestroyTimer());
        canCollide = false;
    }

    private void Flip()
    {
        if(this.transform.parent == bbPos.transform)
        {
            Vector3 pos = transform.localPosition;

            pos.x *= -1;

            transform.localPosition = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && canCollide)
            GetCollected();
    }

    private IEnumerator DestroyTimer()
    {
        commonEnergyPS.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        
        yield return new WaitForSeconds(commonEnergyPS.main.startLifetime.constantMax); 
        Destroy(this.gameObject);
    }

    public float SpeedToGo
    {
        set{this.speedToGo = value;}
    }

    public bool CanGoToBB
    {
        set{this.canGoToBB = value;}
    }

    void OnDisable()
    {
        MovementWithKeyboard.onPlayerFlip -= Flip;
    }
}
