using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspectrumRevealer : MonoBehaviour
{
    private static EspectrumRevealer instance;
    [SerializeField] private GameObject circleGFX; 
    AnimationHandler animationHandler;
    CircleCollider2D circleCollider;
    private Vector2 pos;
    Animator anim;

    private bool isRevealing = false;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        circleCollider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        
        circleCollider.enabled = false;
    }

    void Update()
    {
        isRevealing = Input.GetMouseButtonDown(1);
        
        PositionHandler();
        CircleColliderHandler();
    }

    private void PositionHandler()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos; 
    }
    
    private void CircleColliderHandler()
    {
        if(isRevealing)
            circleCollider.enabled = true;
        else
            circleCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IRevealable revealable = other.GetComponent<IRevealable>();

        if(revealable != null)
        {
            revealable.Reveal();

            if(isRevealing)
                animationHandler.PlayAnimation("IncreaseCircleGfxAnim");
        }
    }
}
