using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Fire")]
    [SerializeField] private float fireRate;
    [SerializeField] private float speedIncreaseFireCollider;
    [SerializeField] private float fireDuration;
    [SerializeField] private ParticleSystem firePS;

    [Header("Damage Player")]
    [SerializeField] private float fireForce;

    [Header("Others")]
    private float time = 0f;
    private Vector2 colliderScale;
    private bool isOnFire = false;
    private bool canStop = false;
    private bool isDisabled;
    private float colliderScaleX;

    void OnEnable()
    {
        firePS.Play();

        Vector2 startLocalScale = transform.localScale;
        startLocalScale.x = 0;
        transform.localScale = startLocalScale;
        
        canStop = false;
    }

    void Start()
    {
        Vector2 startLocalScale = transform.localScale;
        startLocalScale.x = 0;
        transform.localScale = startLocalScale;

        time = 0f;
        colliderScaleX = 0f;
    }

    void Update()
    {
        FireHandler();
        ColliderSizeHandler();
    }

    private void FireHandler()
    {
        time += Time.deltaTime * 7f;//4f

        if(time >= fireRate)
        {
            isOnFire = true;
            canStop = true;
            time = fireRate;
        }
        else
        {
            isOnFire = false;
        }

        if(canStop)
            StartCoroutine(TimeToStopFire());
    }

    private void ColliderSizeHandler()
    {
        colliderScale = transform.localScale;

        colliderScaleX += Time.deltaTime * speedIncreaseFireCollider;
        
        colliderScale.x = colliderScaleX;
        transform.localScale = colliderScale;

        if(colliderScaleX >= 5f)
            colliderScaleX = 5f;
    }

    #region Coroutines
    private IEnumerator TimeToStopFire()
    {
        yield return new WaitForSeconds(fireDuration);
        canStop = false;
        this.enabled = false;
    }
    #endregion

    #region Properties
    public float FireForce
    {
        get{return fireForce;}
    }

    public bool IsOnFire
    {
        get{return this.isOnFire;}
    }

    public bool IsDisabled
    {
        get{return this.isDisabled;}
        set{this.isDisabled = value;}
    }
    #endregion

    void OnDisable()
    {
        isDisabled = true;
        
        firePS.Stop();

        isOnFire = false;

        time = 0f;
        colliderScaleX = 0f;

        Vector2 startLocalScale = transform.localScale;
        startLocalScale.x = 0;
        transform.localScale = startLocalScale;
    }
}
