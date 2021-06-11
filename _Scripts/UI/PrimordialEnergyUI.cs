using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class PrimordialEnergyUI : MonoBehaviour
{
    public static PrimordialEnergyUI instance;
    private Image primordialEnergyImage;
    private TMP_Text primordialEnergyText; //Text
    private bool canShowPrimordialUI;
    private float timeToHide = 3f;

    void Awake()
    {
        instance = this;

        primordialEnergyImage = transform.Find("primordialEnergyImage").GetComponent<Image>();
        primordialEnergyText = transform.Find("primordialEnergyText (TMP)").GetComponent<TMP_Text>();

        primordialEnergyImage.color = new Color(primordialEnergyImage.color.r, primordialEnergyImage.color.g, primordialEnergyImage.color.b, 0f);
        primordialEnergyText.color = new Color(primordialEnergyText.color.r, primordialEnergyText.color.g, primordialEnergyText.color.b, 0f);
    }
    void Start()
    {
        if(StaminaSystem.instance != null)
            StaminaSystem.instance.onStaminaMaximized += OnStaminaChanged_CanShowPrimordialEnergyUIAnim;
    }
    void Update()
    {
        if(!SceneHandler.instance.isLoadingScene)
            primordialEnergyText.text = PrimordialEnergyHandler.instance.PrimordialEnergyAmountInScene.ToString() + "x";

        if(canShowPrimordialUI)
        {
            ShowStaminaUIAnim();
        }
        else
        {
            CanHidePrimordialEnergyUI();
            HidePrimordialEnergyUIAnim();
        }
    }

    private void ShowStaminaUIAnim()
    {
        Color alphaPrimordialEnergyImage = primordialEnergyImage.color;
        Color alphaPrimordialEnergyText = primordialEnergyText.color;

        if(alphaPrimordialEnergyImage.a > 1f)
        {
            alphaPrimordialEnergyImage.a = 1f;
            alphaPrimordialEnergyText.a = 1f;

            canShowPrimordialUI = false;

            timeToHide = 3f;
        }
        else
        {
            alphaPrimordialEnergyImage.a += Time.deltaTime * 0.96f;
            alphaPrimordialEnergyText.a += Time.deltaTime * 0.96f;
        }

        primordialEnergyImage.color = alphaPrimordialEnergyImage;
        primordialEnergyText.color = alphaPrimordialEnergyText;
    }
    private void HidePrimordialEnergyUIAnim()
    {
        Color alphaPrimordialEnergyImage = primordialEnergyImage.color;
        Color alphaPrimordialEnergyText = primordialEnergyText.color;

        if(alphaPrimordialEnergyImage.a < 0f)
        {
            alphaPrimordialEnergyImage.a = 0f;
            alphaPrimordialEnergyText.a = 0f;
        }
        else if(alphaPrimordialEnergyImage.a > 0f && CanHidePrimordialEnergyUI())
        {
            alphaPrimordialEnergyImage.a -= Time.deltaTime * 0.96f;
            alphaPrimordialEnergyText.a -= Time.deltaTime * 0.96f;
        }

        primordialEnergyImage.color = alphaPrimordialEnergyImage;
        primordialEnergyText.color = alphaPrimordialEnergyText;
    }
    
    private void OnStaminaChanged_CanShowPrimordialEnergyUIAnim(object sender, EventArgs e)
    {
        canShowPrimordialUI = true;
    }
    public void CanShowPrimordialUI()
    {
        canShowPrimordialUI = true;
    }
    private bool CanHidePrimordialEnergyUI()
    {
        timeToHide -= Time.deltaTime; 

        if(timeToHide <= 0f)
        {
            timeToHide = 0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDisable()
    {
        if(StaminaSystem.instance != null)
            StaminaSystem.instance.onStaminaMaximized -= OnStaminaChanged_CanShowPrimordialEnergyUIAnim;
    }
}
