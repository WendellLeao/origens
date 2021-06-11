using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    public static CinemachineSwitcher instance;
    [SerializeField] private Animator animator;
    CinemachineFramingTransposer cinemachineFramingTransposerBB;
    CinemachineFramingTransposer cinemachineFramingTransposerBP;

    [Header("Timer")]
    private bool canEnableLookaheadTime_BloquinhoBranco = false;
    private bool canEnableLookaheadTime_BloquinhoPreto = false;
    private bool canDelayToEnableLookahead = false;
    private float smoothness = 0.08f;
    private float time = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(BloquinhoBrancoController.instance != null)
        {
            BloquinhoBrancoController.instance.onBloquinhosChanged += SwitchCamera;
            
            cinemachineFramingTransposerBB = 
            CamAssets.instance.virtualCameraBB.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if(BloquinhoPretoController.instance != null)
        {
            CamAssets.instance.virtualCameraBP.Follow = BloquinhoPretoController.instance.transform;

            cinemachineFramingTransposerBP =
            CamAssets.instance.virtualCameraBP.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        cinemachineFramingTransposerBB.m_LookaheadTime = 0.2f;
        cinemachineFramingTransposerBP.m_LookaheadTime = 0.4f;
    }

    void Update()
    {
        if(canDelayToEnableLookahead)
            StartCoroutine(DelayEnableLookahead());

        if(canEnableLookaheadTime_BloquinhoBranco && BloquinhoBrancoController.instance.movementWithKeyboard.GetMoveInputHorizontal() != 0f)
        {
            EnableLookaheadTime_BloquinhoBranco(0.2f);
            canEnableLookaheadTime_BloquinhoPreto = false;
        }
    }

    private void SwitchCamera()
    {
        time = 0f;

        cinemachineFramingTransposerBB.m_LookaheadTime = 0f;

        if(BloquinhoBrancoController.instance != null && BloquinhoBrancoController.instance.state != BloquinhoBrancoController.State.Dead)
        {
            if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            {
                animator.Play("BB_Vcam");

                canDelayToEnableLookahead = true;
            }
            else
            {
                animator.Play("BP_Vcam");
            }
        }
    }

    private void EnableLookaheadTime_BloquinhoBranco(float lookaheadTime)
    {
        time += Time.deltaTime * smoothness;
        cinemachineFramingTransposerBB.m_LookaheadTime = time;

        if(time >= lookaheadTime)
        {
            time = lookaheadTime;
            cinemachineFramingTransposerBB.m_LookaheadTime = lookaheadTime;
            canEnableLookaheadTime_BloquinhoBranco = false;
            canDelayToEnableLookahead = false;
        }
    }

    IEnumerator DelayEnableLookahead()
    {
        yield return new WaitForSeconds(0.8f);  
        canEnableLookaheadTime_BloquinhoBranco = true;
        canDelayToEnableLookahead = false;
    }

    void OnDisable()
    {
        if(BloquinhoBrancoController.instance != null)
            BloquinhoBrancoController.instance.onBloquinhosChanged -= SwitchCamera;
    }
}
