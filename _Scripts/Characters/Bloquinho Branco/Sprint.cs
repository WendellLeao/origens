using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementWithKeyboard))]
public class Sprint : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private float maxRunSpeed;

    [Header("Increase Speed With Time (0 = with no time)")]
    [SerializeField] private float speedToReachMax;
    private bool isDiminished = true;
    private bool isRunning;
    private float minRunSpeed;
    private float runSpeed;
    MovementWithKeyboard movementBase;
    private bool canSprintInTheAir;
    private bool hasJustFell = false;

    void Awake()
    {
        movementBase = GetComponent<MovementWithKeyboard>();
    }
    void Start()
    {
        minRunSpeed = movementBase.GetWalkSpeed();
        runSpeed = minRunSpeed;
    }
    void Update()
    {
        CheckIfIsRunning();
        
        if(GetComponent<BloquinhoBrancoController>().IsGrounded())
        {
            canSprintInTheAir = 
                Input.GetKey(KeyCode.LeftShift) 
                && StaminaSystem.instance.GetStaminaAmount() >= 0f 
                && GetComponent<MovementWithKeyboard>().GetMoveInputHorizontal() != 0; 
        }

        SprintHandler();

        if(runSpeed <= minRunSpeed)
        {
            isDiminished = true;
            hasJustFell = false;
        }
        else
        {
            isDiminished = false;

            if(!GetComponent<BloquinhoBrancoController>().IsGrounded())
                hasJustFell = true;
        }

        if(!BloquinhoBrancoController.instance.isBloquinhoBranco)
        {
            isRunning = false;
            runSpeed = minRunSpeed;
        }
    }
    void FixedUpdate()
    {
        if(StaminaSystem.instance.GetStaminaAmount() <= 0f || GetComponent<MovementWithKeyboard>().GetMoveInputHorizontal() == 0)
            isRunning = false;

        if(isRunning)
        {
            if(GetComponent<BloquinhoBrancoController>().IsGrounded())
            {
                if(GetComponent<Rigidbody2D>().velocity.y < 1f)
                {
                    if(!hasJustFell)
                        GetComponent<AnimationHandler>().PlayAnimation("WalkToRunAnim");
                    else
                        GetComponent<AnimationHandler>().PlayAnimation("BBRunAnim");
                }
            }

            movementBase.MovementHandler(runSpeed);
        }
    }

    private void CheckIfIsRunning()
    {
        if(GetComponent<BloquinhoBrancoController>().IsGrounded())
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if(StaminaSystem.instance.GetStaminaAmount() > 0f && movementBase.GetMoveInputHorizontal() != 0f)
                {
                    isRunning = true;
                    GetComponent<BloquinhoBrancoController>().CreateDust();       
                }
            }
            else
            {
                isRunning = false;
            }
        }    
        else
        {
            isRunning = false;
        }

        if(StaminaSystem.instance.GetStaminaAmount() <= 0f)
            isRunning = false;
    }

    void SprintHandler()
    {
        if(isRunning)
        {
            if(speedToReachMax > 0)
            {
                runSpeed = IncreaseSpeedWithTime();
            }
            else
            {
                runSpeed = maxRunSpeed;
            }

            StaminaSystem.instance.DecreaseStamina();
        }
        else
        {
            if(GetComponent<BloquinhoBrancoController>().IsGrounded()) //If BB is sprinting in the air he doesn't slow down
            {
                if(speedToReachMax > 0)
                {
                    runSpeed = DecreaseSpeedWithTime();
                }
                else
                {
                    runSpeed = minRunSpeed;
                }
            }
        }

        movementBase.SetWalkSpeed(runSpeed);
    }

    private float IncreaseSpeedWithTime()
    {
        if(runSpeed < maxRunSpeed)
        {
            runSpeed += Time.fixedDeltaTime * speedToReachMax;
        }

        if(runSpeed >= maxRunSpeed)
        {
            runSpeed = maxRunSpeed;
        }

        return runSpeed;
    }
    private float DecreaseSpeedWithTime()
    {
        if(runSpeed > minRunSpeed)
        {
            runSpeed -= Time.fixedDeltaTime * speedToReachMax * 2f;
        }

        if(runSpeed <= minRunSpeed)
        {
            runSpeed = minRunSpeed;
        }

        return runSpeed;
    }

    public float GetRunSpeed()
    {
        return runSpeed;
    }

    public void SetRunSpeed(float value)
    {
        this.runSpeed = value;
    }

    public float GetMinRunSpeed()
    {
        return this.minRunSpeed;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void SetIsRunning(bool value)
    {
        this.isRunning = value;
    }

    public bool CanSprintInTheAir()
    {
        return canSprintInTheAir;
    }

    public bool IsDiminished
    {
        get{return isDiminished;}
    }

    public bool HasJustFell
    {
        get{return hasJustFell;}
    }
}
