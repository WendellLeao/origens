using System.Collections;
using UnityEngine;
using System;

public class BloquinhoBrancoController : MonoBehaviour
{
    public enum State { Alive, Dead, OnCutscene ,LevelComplete}
    public static BloquinhoBrancoController instance;
    public event Action onBloquinhosChanged, onLevelComplete;

    [Header("Stamina System")]
    [SerializeField] private int maxStamina;
    StaminaSystem staminaSystem;

    [Header("Bloquinho Branco Components")]
    [HideInInspector] public MovementWithKeyboard movementWithKeyboard;
    ImproveJumpPhysics improveJumpPhysics;
    DamageHandler damageHandler;
    [HideInInspector] public Rigidbody2D bbBody;
    Sprint sprint;
    [HideInInspector] public Jump jump;
    [HideInInspector] public Dash dash;
    
    [Header("Check Ground")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsMovingPlatform;
    private BoxCollider2D boxCollider2D;

    [Header("Transforms")]
    [HideInInspector] public Transform bpPos;
    private Transform bbPos;
    public Transform bpPosInBB;

    [Header("Bloquinho Preto Spawn")]
    [SerializeField] private GameObject bloquinhoPretoObj;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem dustPS;
    private Transform startDustPsPosition;
    
    [Header("Objects")]
    [SerializeField] private GameObject bbEyesObj;
    [SerializeField] private GameObject ropeObj;
    
    [Header("Others")]
    [HideInInspector] public State state;
    [HideInInspector] public bool isBloquinhoBranco = true;
    private bool canMove = true;
    private bool isBeingGrabbed = false;
    private bool canBeGrabbed = true;
    private int pressedKey;
    private float timeToResetPressedKey;

    void Awake()
    {
        instance = this;

        staminaSystem = new StaminaSystem(maxStamina);

        movementWithKeyboard = GetComponent<MovementWithKeyboard>();
        improveJumpPhysics = GetComponent<ImproveJumpPhysics>();    
        damageHandler = GetComponent<DamageHandler>();
        bbBody = GetComponent<Rigidbody2D>();
        sprint = GetComponent<Sprint>();
        jump = GetComponent<Jump>();
        dash = GetComponent<Dash>();

        boxCollider2D = GetComponent<BoxCollider2D>();
        
        bbPos = GetComponent<Transform>();
        //bpPos = FindObjectOfType<BloquinhoPretoController>().GetComponent<Transform>();

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level00" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Finding Bloquinho Preto")
        {
            GameObject bloquinhoPretoClone = Instantiate(bloquinhoPretoObj, bpPosInBB.position, bpPosInBB.rotation);
            bloquinhoPretoClone.name = "BloquinhoPreto";

            bpPos = bloquinhoPretoClone.GetComponent<Transform>();
        }

        GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        bbEyesObj.GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
        bbEyesObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
    void Start()
    {
        isBloquinhoBranco = true;
        state = State.Alive;

        if(CheckPointHandler.instance != null) // or level is not "Finding Bloquinho Preto"
            transform.position = CheckPointHandler.instance.LastCheckPointPos;
        
        startDustPsPosition = dustPS.transform;

        bbEyesObj.SetActive(true);

        EnableComponents();
    }
    void OnEnable()
    {
        onBloquinhosChanged += SwitchBloquinhos;

        if(damageHandler != null)
            damageHandler.onPlayerDied += OnPlayerDead_DisableBB;
        
        if(dash != null)
            dash.onDash += EnableBB;

        // if(CutsceneHandler.instance != null)
        // {
        //     CutsceneHandler.instance.OnCutscenePlay += OnCutscenePlay_DisableMovement;
        //     CutsceneHandler.instance.OnCutsceneStop += OnCutsceneStop_EnableMovement;
        // }
    }
    void Update()
    {
        if(state == State.Alive)
        {
            if(Input.GetKeyDown(KeyCode.Q) && Time.timeScale >= 1f && BloquinhoPretoController.instance != null) 
            {
                if(Dash.instance != null)
                {
                    if(!dash.IsDashing())
                        onBloquinhosChanged?.Invoke();
                }
                else
                {
                    onBloquinhosChanged?.Invoke();
                }
            } 

            if(isBloquinhoBranco && !canMove)
            {
                if(IsGrounded())
                    GetComponent<AnimationHandler>().PlayAnimation("BBIdleAnim");
                else if(!IsGrounded() && isBeingGrabbed)
                    GetComponent<AnimationHandler>().PlayAnimation("BBJumpAnim");
            }

            ReleaseGrabInput();

            if(canBeGrabbed)
                GrabInTheEnemyHandler();
            else
                isBeingGrabbed = false;

            if (!isBeingGrabbed)
            {
                if (isBloquinhoBranco)
                {
                    canMove = true;
                    EnableMovement();
                }

                if(!IsOnMovingPlatform())
                {
                    this.transform.parent = null;

                    if(bpPos != null)
                        bpPos.parent = null;
                }
            }

            if(isBloquinhoBranco)
            {
                if(dash != null)
                {
                    if(!dash.IsDashing() && canMove)
                        movementWithKeyboard.enabled = true;
                    else
                        movementWithKeyboard.enabled = false;

                    if(!dash.IsDashing())
                    {
                        if(canBeGrabbed)
                            GrabInTheEnemyHandler();
                    }
                }
                else
                {
                    if(canMove)
                        movementWithKeyboard.enabled = true;
                    else
                        movementWithKeyboard.enabled = false;
                }
            }
            else
            {
                if(canMove || !isBeingGrabbed)
                    bbBody.gravityScale = 2.2f;
            }
        }

        CutsceneMovementHandler();
        if(state == State.OnCutscene)
        {
            bbBody.velocity = new Vector2(0f, bbBody.velocity.y);
            GetComponent<AnimationHandler>().PlayAnimation("BBIdleAnim");
        }

        if(isBloquinhoBranco)
        {
            if(state != State.Dead)
                bbEyesObj.SetActive(true);
        }
        else
        {
            bbEyesObj.SetActive(false);

            if(sprint != null)
            {
                sprint.SetIsRunning(false);
                sprint.SetRunSpeed(sprint.GetMinRunSpeed());
            }
        }

        /////////////////////FOR DEBUG////////////////////////////////
        // if(Input.GetKeyDown(KeyCode.L))
        //     onLevelComplete?.Invoke();

        if(Time.deltaTime <= 0f) //if it is paused
            DisableComponents();
    }
    void FixedUpdate()
    {
        if(state == State.Alive)
        {
            if(!isBloquinhoBranco)
                movementWithKeyboard.Skid();
        }

        if(state == State.LevelComplete)
        {
            isBloquinhoBranco = true;
            
            DisableComponents();

            if(BloquinhoPretoController.instance != null) //if its not the level 00
            {
                CamAssets.instance.virtualCameraBB.Follow = null;
                CinemachineSwitcher.instance.gameObject.SetActive(false);
            }

            if(IsGrounded())
                GetComponent<AnimationHandler>().PlayAnimation("BBWalkAnim");
            else
                GetComponent<AnimationHandler>().PlayAnimation("BBJumpAnim");
            
            bbBody.velocity = new Vector2(movementWithKeyboard.GetWalkSpeed(), bbBody.velocity.y);

            onLevelComplete?.Invoke();
        }
    }

    private void SwitchBloquinhos()
    {
        if(BloquinhoPretoController.instance != null)
        {
            AudioHandler.instance.Play("TrocaDeAlmaFX");

            isBloquinhoBranco = !isBloquinhoBranco;

            if(isBloquinhoBranco)
            {
                EnableComponents();
            }
            else
            {
                DisableComponents();
                GetComponent<AnimationHandler>().PlayAnimation("BBSleepingAnim");
            }
        }
    }
    public void ForceSwitchBloquinhos()
    {
        isBloquinhoBranco = true;
        EnableComponents();

        if(BloquinhoPretoController.instance != null)
            BloquinhoPretoController.instance.DisableComponents();
    }

    private void EnableComponents()
    {
        if(CursorHandler.instance != null)
            CursorHandler.instance.ShowCursor();

        GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        bbEyesObj.GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
        bbEyesObj.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //bbEyesObj.SetActive(true);

        if(state != State.OnCutscene)
            EnableMovement();
    }
    private void DisableComponents()
    {
        DisableMovement();

        movementWithKeyboard.SetWalkSpeed(movementWithKeyboard.GetStartWalkSpeed());

        GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        bbEyesObj.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        bbEyesObj.GetComponent<SpriteRenderer>().sortingOrder = 2;

        // if(state == State.Dead || !isBloquinhoBranco)
        //     bbEyesObj.SetActive(false);

        bbBody.sharedMaterial = null;        
    }

    public void EnableMovement()
    {
        if(canMove)
        {
            improveJumpPhysics.enabled = true;
            sprint.enabled = true;
            jump.enabled = true;
        }
    }
    public void DisableMovement()
    {
        movementWithKeyboard.enabled = false;
        improveJumpPhysics.enabled = false;
        sprint.enabled = false;
        jump.enabled = false;
    }

    private void CutsceneMovementHandler()
    {
        if(CutsceneHandler.instance != null)
        {
            if (CutsceneHandler.instance.IsPlayingCutscene)
            {
                state = State.OnCutscene;
                OnCutscenePlay_DisableMovement();
            }
            else if(state != State.OnCutscene && state != State.LevelComplete)
            {
                OnCutsceneStop_EnableMovement();
            }
        }
    }

    private void OnCutscenePlay_DisableMovement()
    {
        DisableMovement();
    }
    private void OnCutsceneStop_EnableMovement()
    {
        state = State.Alive;
        EnableMovement();
    }

    private void EnableBB()
    {
        isBloquinhoBranco = true;
        
        EnableComponents();
    }
    private void OnPlayerDead_DisableBB()
    {
        state = State.Dead;

        DisableComponents();

        CamAssets.instance.virtualCameraBB.Follow = null;
        CinemachineSwitcher.instance.gameObject.SetActive(false);

        //if(!damageHandler.DeadByDeadZoneGap)
        bbEyesObj.SetActive(false);
        
        ropeObj.SetActive(false);

        if(!IsGrounded())
        {
            isBloquinhoBranco = true;

            //bbBody.velocity = new Vector2(0f, bbBody.velocity.y);
        }
        else
        {
            if(isBloquinhoBranco)
                GetComponent<AnimationHandler>().PlayAnimation("BBIdleAnim");

            bbBody.velocity = new Vector2(0f, bbBody.velocity.y);
        }

        //bbBody.velocity = new Vector2(0f, bbBody.velocity.y);
    }

    public bool IsGrounded()
    {
        float extraHeightText = 0.06f; //0.2f  0.06f
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, whatIsGround);
        Color rayColor;
        
        if(raycastHit.collider != null)
            rayColor = Color.green;
        else    
            rayColor = Color.red;

        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }
    public bool IsOnMovingPlatform()
    {
        float extraHeightText = 0.06f; //0.2f  0.06f
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, whatIsMovingPlatform);
        // Color rayColor;
        
        // if(raycastHit.collider != null)
        //     rayColor = Color.green;
        // else    
        //     rayColor = Color.red;

        // Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        // Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        // Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }
    public void CreateDust()
    {
        if(jump.IsJumping && movementWithKeyboard.GetMoveInputHorizontal() == 0f)
        {
            dustPS.transform.localPosition = new Vector3(0, startDustPsPosition.localPosition.y, startDustPsPosition.localPosition.z);
        }
        else if(IsGrounded())
        {
            dustPS.transform.localPosition = new Vector3(startDustPsPosition.localPosition.x, startDustPsPosition.localPosition.y, startDustPsPosition.localPosition.z);
        }

        dustPS.Play();
    }
    
    private void GrabInTheEnemyHandler()
    {
        if(isBeingGrabbed)
        {
            canMove = false;

            if (!canMove)
            {
                if (isBloquinhoBranco)
                    DisableMovement();

                bbBody.velocity = new Vector2(0f, bbBody.velocity.y);
            }
        }
    }
    private void ReleaseGrabInput()
    {
        if(!canMove && isBloquinhoBranco && isBeingGrabbed)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                timeToResetPressedKey = 0.4f;
                pressedKey++;
            }

            //Timer to reset
            timeToResetPressedKey -= Time.deltaTime;

            if(timeToResetPressedKey <= 0f)
            {
                pressedKey = 0;
                timeToResetPressedKey = 0f;
            }

            //If press space 5 times
            if(pressedKey >= 5)
            {
                StartCoroutine(TimerToBeGrabbedAgain());
                isBeingGrabbed = false;
                pressedKey = 0;
            }
        }
        else
        {
            pressedKey = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "FinalTrigger")
            state = State.LevelComplete;
    }

    public static bool IsBloquinhoBrancoStatic()
    {
        return instance.isBloquinhoBranco;
    }

    public bool IsBeingGrabbed
    {
        get{return isBeingGrabbed;}
        set{this.isBeingGrabbed = value;}
    }

    IEnumerator TimerToBeGrabbedAgain()
    {
        canBeGrabbed = false;
        yield return new WaitForSeconds(0.5f);
        canBeGrabbed = true;
    }

    void OnDisable()
    {
        onBloquinhosChanged -= SwitchBloquinhos;
        
        if(damageHandler != null)
            damageHandler.onPlayerDied -= OnPlayerDead_DisableBB;
        
        if(dash != null)
            dash.onDash -= EnableBB;

        // if(CutsceneHandler.instance != null)
        // {
        //     CutsceneHandler.instance.OnCutscenePlay -= OnCutscenePlay_DisableMovement;
        //     CutsceneHandler.instance.OnCutsceneStop -= OnCutsceneStop_EnableMovement;
        // }
    }
}
