using UnityEngine;

public class BloquinhoPretoController : MonoBehaviour
{
    public static BloquinhoPretoController instance;

    [Header("Bloquinho Preto Components")]
    MovementWithMouse movementWithMouse;
    EspectrumRevealer espectrumRevealer;
    Telecinese telecinese;
    Rigidbody2D bpBody;

    [Header("Objects")]
    [SerializeField] private GameObject telecineseObj;
    [SerializeField] private GameObject bpEyes;

    [Header("Transforms")]
    private Transform bbPos;
    public Transform ropePointBP;
    private Transform bpPos;
    private Vector3 startRotation;

    // [Header("GameObjects")]
    // [SerializeField] private GameObject telecineseObj;
    // [SerializeField] private GameObject espectrumRevealerObj;

    void Awake()
    {
        instance = this;
        
        movementWithMouse = GetComponent<MovementWithMouse>();
        espectrumRevealer = GetComponent<EspectrumRevealer>();
        telecinese = GetComponent<Telecinese>();
        bpBody = GetComponent<Rigidbody2D>();
        
        bpPos = GetComponent<Transform>();
        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();
    }
    void Start()
    {
        BloquinhoBrancoController.instance.onBloquinhosChanged += SwitchBloquinhos;
        
        if(DamageHandler.instance != null)
            DamageHandler.instance.onPlayerDied += DisableBP;

        if(Dash.instance != null)
            Dash.instance.onDash += DisableBP;

        startRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        bpEyes.SetActive(false);

        DisableBP();
    }
    void Update()
    {
        if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            transform.rotation = Quaternion.Euler(startRotation);

            if(Mathf.Abs(BloquinhoBrancoController.instance.bbBody.velocity.x) >= 1f || Mathf.Abs(BloquinhoBrancoController.instance.bbBody.velocity.y) >= 1f)
                transform.position = BloquinhoBrancoController.instance.bpPosInBB.position;

            telecineseObj.SetActive(true);

            // if(BloquinhoBrancoController.instance.IsOnMovingPlatform())//BloquinhoBrancoController.instance.movementWithKeyboard.IsOnPlatform()
            //     transform.parent = BloquinhoBrancoController.instance.platformObject.transform;
            // else
            //     transform.parent = null;
        }
        else
        {
            bpEyes.SetActive(false);
            
            transform.position = BloquinhoBrancoController.instance.bpPosInBB.position;
            transform.rotation = BloquinhoBrancoController.instance.bpPosInBB.rotation;

            // transform.parent = null;
            
            telecineseObj.SetActive(false);

            FlipHandler();
        }

        if(BloquinhoBrancoController.instance.state == BloquinhoBrancoController.State.Dead)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
    void FixedUpdate()
    {
        #region Commented Code
        // if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        // {
        //     //EnableVcam();

        //     //transform.rotation = Quaternion.Euler(startRotation);

        //     // if(!movementWithMouse.IsOutOfMinimumArea())// || BloquinhoBrancoController.instance.movementWithKeyboard.IsOnPlatform()
        //     // {
        //     //     //transform.parent = bbPos;
                
        //     //     // if(Mathf.Abs(BloquinhoBrancoController.instance.bbBody.velocity.x) >= 1f || Mathf.Abs(BloquinhoBrancoController.instance.bbBody.velocity.y) >= 1f)
        //     //     //     transform.position = BloquinhoBrancoController.instance.bpPosInBB.position;
        //     // }
        //     // else
        //     // {
        //     //     // if(!BloquinhoBrancoController.instance.movementWithKeyboard.IsOnPlatform())
        //     //     //     transform.parent = null;
        //     // }
        // }
        // else
        // {
        //     // if(!BloquinhoBrancoController.instance.movementWithKeyboard.IsOnPlatform())
        //     //     transform.parent = null;
            
        //     // transform.position = BloquinhoBrancoController.instance.bpPosInBB.position;
        //     // transform.rotation = BloquinhoBrancoController.instance.bpPosInBB.rotation;

        //     //DisableVcam();
        //     FlipHandler();
        // }
        #endregion
    }

    public void SwitchBloquinhos()
    {
        if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            EnableComponents();
        else
            DisableComponents();
    }

    private void EnableComponents()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoPreto";
        //GetComponent<SpriteRenderer>().color = Color.white; ///

        bpEyes.SetActive(true);

        //telecineseObj.SetActive(true);

        movementWithMouse.enabled = true;
    }
    public void DisableComponents()
    {
        if(BloquinhoBrancoController.instance != null)
        {
            if(!BloquinhoBrancoController.instance.IsGrounded())
                GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";
        }
        
        //GetComponent<SpriteRenderer>().color = Color.black; ///
        bpEyes.SetActive(false);
        
        //telecineseObj.SetActive(false);

        movementWithMouse.enabled = false;

        // if(CamAssets.instance != null)
        //     CamAssets.instance.virtualCameraBP.m_Lens.OrthographicSize = 6f;

        //movementWithMouse.minMouseDistX = 4;
        //movementWithMouse.minMouseDistY = 4;
    }
    
    private void EnableVcam()
    {
        if(CamAssets.instance != null)
        {
            CamAssets.instance.virtualCameraBP.enabled = true;
            CamAssets.instance.virtualCameraBP.Follow = bpPos;
        }
    }
    private void DisableVcam()
    {
        if(CamAssets.instance != null)
        {
            CamAssets.instance.virtualCameraBP.enabled = false;
            CamAssets.instance.virtualCameraBP.Follow = bbPos;
        }
    }
    
    private void DisableBP()
    {
        DisableComponents();
        //DisableVcam();
    }
    
    public void FlipHandler()
    {
        if (BloquinhoBrancoController.instance.movementWithKeyboard.IsFacingRight() && !movementWithMouse.IsFacingRight() 
        || !BloquinhoBrancoController.instance.movementWithKeyboard.IsFacingRight() && movementWithMouse.IsFacingRight())
        {
            movementWithMouse.Flip();
        }
    }

    void OnDisable()
    {
        BloquinhoBrancoController.instance.onBloquinhosChanged -= SwitchBloquinhos;
        
        if(DamageHandler.instance != null)
            DamageHandler.instance.onPlayerDied -= DisableBP;

        if(Dash.instance != null)
            Dash.instance.onDash -= DisableBP;
    }
}
