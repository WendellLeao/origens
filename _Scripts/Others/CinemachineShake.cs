using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance {get ; private set; }
    [SerializeField] private CinemachineVirtualCamera vCamBB, vCamBP;
    private float shakeTimer;
    
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin_VcamBB = 
                    vCamBB.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin_VcamBP = 
                    vCamBP.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    cinemachineBasicMultiChannelPerlin_VcamBB.m_AmplitudeGain = 0f;
                    cinemachineBasicMultiChannelPerlin_VcamBP.m_AmplitudeGain = 0f;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin_VcamBB =
            vCamBB.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin_VcamBP =
            vCamBP.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            cinemachineBasicMultiChannelPerlin_VcamBB.m_AmplitudeGain = intensity;
            cinemachineBasicMultiChannelPerlin_VcamBP.m_AmplitudeGain = 0f;
        }
        else
        {
            cinemachineBasicMultiChannelPerlin_VcamBP.m_AmplitudeGain = intensity;
            cinemachineBasicMultiChannelPerlin_VcamBB.m_AmplitudeGain = 0f;
        }

        shakeTimer = time;
    }
}
