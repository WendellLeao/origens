using System.Collections;
using UnityEngine;
using System;
public class Dash : MonoBehaviour
{
    public static Dash instance;
    public event Action onDash;

    [Header("Components")]
    Rigidbody2D body;
    
    [Header("Velocity")]
    [SerializeField] private float pushForceX;
    [SerializeField] private float pushForceY;
    [SerializeField] private float maxMouseDistanceX;
    [SerializeField] private float maxMouseDistanceY;

    [Header("Game Objects")]
    [SerializeField] private GameObject dashTrailPf;
    [SerializeField] private GameObject dashTriggerPf;
    private GameObject dashTrigger;

    [Header("Others")]
    private bool isDragging = false;
    private bool isDashing = false;
    private Vector2 mousePosition;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 forceY;
    private Vector2 forceX;
    private float distance;
    private float distanceMouse;
    private bool canStopTime = false;
    private LineRenderer lineRenderer;
    Color startLineRendererColor;
    
    void Awake()
    {
        instance = this;
        
        body = GetComponent<Rigidbody2D>();

        GameObject dashTrailClone = Instantiate(dashTrailPf, transform.position, transform.rotation);
        dashTrailClone.transform.parent = this.transform;
        dashTrailClone.name = "DashTrail";

        lineRenderer = dashTrailClone.GetComponent<LineRenderer>();
        startLineRendererColor = lineRenderer.startColor;

        GameObject dashTriggerClone = Instantiate(dashTriggerPf, transform.position, transform.rotation);
        dashTriggerClone.transform.parent = this.transform;
        dashTriggerClone.name = "DashTrigger";

        dashTrigger = dashTriggerClone;
    }
    void Update()
    {
        if(!GameHandler.IsGamePaused())
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            distanceMouse = Vector2.Distance(transform.position, mousePosition);
            
            if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            {
                if(distanceMouse <= 0.7f)
                {
                    if(Input.GetMouseButtonDown(0) && GetComponent<BloquinhoBrancoController>().IsGrounded() 
                    && StaminaSystem.instance.GetStaminaAmount() >= StaminaSystem.instance.GetMaxStamina())
                        OnDragStart();
                }

                if(Input.GetMouseButtonUp(0) && GetComponent<BloquinhoBrancoController>().IsGrounded() && isDragging)
                    OnDragEnd();

                if(isDragging)
                    OnDrag();

                // if(Input.GetMouseButtonUp(0) && GetComponent<BloquinhoBrancoController>().IsGrounded() && StaminaSystem.instance.GetStaminaAmount() <= StaminaSystem.instance.GetMaxStamina())
                //     StaminaBar.ShowStaminaUIAnimStatic();
            }  

            if(isDashing)
            {
                dashTrigger.SetActive(true);
                
                GetComponent<AnimationHandler>().PlayAnimation("BBJumpAnim");

                if(Input.GetButtonDown("Jump") && BloquinhoBrancoController.IsBloquinhoBrancoStatic() && GetComponent<BloquinhoBrancoController>().IsGrounded())
                    isDashing = false;
            }
            else
            {
                dashTrigger.SetActive(false);   
            }
        }

        NormalizeTime();
        DragIndicatorLayerHandler();
    }
    void FixedUpdate()
    {
        if(Input.GetMouseButtonUp(0) && GetComponent<BloquinhoBrancoController>().IsGrounded() && isDragging)
            body.velocity = new Vector2(forceX.x, forceY.y);

        if(Mathf.Abs(body.velocity.x) <= 3f && isDashing)
            isDashing = false;
    }

    private void OnDragStart()
    {
        isDragging = true;

        startPoint = transform.position;

        //Enable drag indicator
        lineRenderer.enabled = true;

        DoSlowMotion();
    }
    private void OnDrag()
    {
        float distanceMouseX = Mathf.Abs(transform.position.x - mousePosition.x);
        float distanceMouseY = Mathf.Abs(transform.position.y - mousePosition.y);

        endPoint = mousePosition;
        
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        
        if(distanceMouseX <= maxMouseDistanceX)
        {
            forceX = direction * distance * pushForceX;

            if(distanceMouseY <= maxMouseDistanceY)
            {
                forceY = direction * distance * pushForceY;
                lineRenderer.startColor = startLineRendererColor;
            }
            else
            {
                lineRenderer.startColor = Color.red;
            }
        }
        else
        {
            lineRenderer.startColor = Color.red;
        }

        CamAssets.instance.virtualCameraBP.m_Lens.OrthographicSize = 5.8f;
        
        Vector2 startDragPoint = new Vector2(transform.position.x, transform.position.y - 0.15f);

        lineRenderer.SetPosition(0, startDragPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
    private void OnDragEnd()
    {
        onDash?.Invoke();

        isDragging = false;
        isDashing = true;
        
        canStopTime = false;

        lineRenderer.enabled = false;

        StaminaSystem.instance.DecreaseStamina(StaminaSystem.instance.GetMaxStamina());

        FlipHandler();
    }

    private void DoSlowMotion()
    {
        canStopTime = true;

        if(canStopTime && Time.timeScale <= 99f)
        {
            Time.timeScale = 0.15f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
    private void NormalizeTime()
    {
        if(!GameHandler.IsGamePaused())
        {
            if(!canStopTime)
            {
                Time.timeScale += (1f / 0.2f) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime += (0.01f / 0.2f) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.02f);
            }
        }
        else
        {
            isDragging = false;
            lineRenderer.enabled = false;
        }
    }

    private void FlipHandler()
    {
        if (transform.position.x < mousePosition.x && GetComponent<MovementWithKeyboard>().IsFacingRight() 
        || transform.position.x > mousePosition.x && !GetComponent<MovementWithKeyboard>().IsFacingRight())
            GetComponent<MovementWithKeyboard>().Flip();
    }

    private void DragIndicatorLayerHandler()
    {
        if(isDragging)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Over";
            lineRenderer.sortingLayerName = "Over";
            lineRenderer.sortingOrder = -1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
            lineRenderer.sortingLayerName = "BloquinhoBranco"; 
            lineRenderer.sortingOrder = -1;
        }
    }
    
    public bool IsDashing()
    {
        return isDashing;
    }
    public bool IsDragging()
    {
        return isDragging;
    }
}
