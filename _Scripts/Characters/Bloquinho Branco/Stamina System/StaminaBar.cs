using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class StaminaBar : MonoBehaviour
{
    private static StaminaBar instance;
    private Color originalStaminaText;
    private Image barImage;
    private TMP_Text staminaText;

    private bool canShowStaminaUI;
    private float timeToHide;

    void Awake()
    {
        instance = this;

        barImage = transform.Find("bar").GetComponent<Image>();
        staminaText = transform.Find("staminaText (TMP)").GetComponent<TMP_Text>();
        
        canShowStaminaUI = false;
        timeToHide = 3f;
    }
    void Start()
    {
        StaminaSystem.instance.onStaminaChanged += OnStaminaChanged_CanShowStaminaUI;

        barImage.color = new Color(barImage.color.r, barImage.color.g, barImage.color.b, 0f);
        staminaText.color = new Color(staminaText.color.r, staminaText.color.g, staminaText.color.b, 0f);

        originalStaminaText = staminaText.color;
    }
    void OnEnable()
    {
        //StaminaSystem.instance.onStaminaChanged += OnStaminaChanged_CanShowStaminaUI;
    }
    void Update()
    {
        barImage.fillAmount = StaminaSystem.instance.GetStaminaNormalized();

        if(canShowStaminaUI)
        {
            ShowStaminaUIAnim();
        }
        else
        {
            CanHideStaminaUI();
            HideStaminaUIAnim();
        }

        //if trying to run with no stamina  
        if(StaminaSystem.instance.GetStaminaAmount() <= 0f && Input.GetKey(KeyCode.LeftShift))
            canShowStaminaUI = true;

        ChangeStaminaTextColor();
    }

    private void ShowStaminaUIAnim()
    {
        Color alphaBarImage = barImage.color;
        Color alphaStaminaText = staminaText.color;

        if(alphaBarImage.a > 1f)
        {
            alphaBarImage.a = 1f;
            alphaStaminaText.a = 1f;

            canShowStaminaUI = false;

            timeToHide = 3f;
        }
        else
        {
            alphaBarImage.a += Time.deltaTime * 0.96f;
            alphaStaminaText.a += Time.deltaTime * 0.96f;
        }

        barImage.color = alphaBarImage;
        staminaText.color = alphaStaminaText;
    }
    private void HideStaminaUIAnim()
    {
        Color alphaBarImage = barImage.color;
        Color alphaStaminaText = staminaText.color;

        if(alphaBarImage.a < 0f)
        {
            alphaBarImage.a = 0f;
            alphaStaminaText.a = 0f;
        }
        else if(alphaBarImage.a > 0f && CanHideStaminaUI())
        {
            alphaBarImage.a -= Time.deltaTime * 0.96f;
            alphaStaminaText.a -= Time.deltaTime * 0.96f;
        }

        barImage.color = alphaBarImage;
        staminaText.color = alphaStaminaText;
    }
    
    private void OnStaminaChanged_CanShowStaminaUI(object sender, EventArgs e)
    {
        canShowStaminaUI = true;
    }
    private bool CanHideStaminaUI()
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

    private void ChangeStaminaTextColor()
    {
        if(StaminaSystem.instance.GetStaminaAmount() <= 0)
        {
            staminaText.color = new Color(Color.red.r, 0f, 0f, staminaText.color.a);
        }
        else
        {
            staminaText.color = new Color(originalStaminaText.r, originalStaminaText.g, originalStaminaText.b, staminaText.color.a);
        }
    }
    
    public static void ShowStaminaUIAnimStatic()
    {
        instance.canShowStaminaUI = true;
    }

    void OnDisable()
    {
        barImage.color = new Color(barImage.color.r, barImage.color.g, barImage.color.b, 0f);
        staminaText.color = new Color(staminaText.color.r, staminaText.color.g, staminaText.color.b, 0f);
        StaminaSystem.instance.onStaminaChanged -= OnStaminaChanged_CanShowStaminaUI;
        timeToHide = 0f;
    }
}
