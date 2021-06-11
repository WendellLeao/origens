using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CheckPointUI : MonoBehaviour
{
    private static CheckPointUI instance;
    private Text checkPointUIText;

    private bool canShowCheckPointUI;
    private float timeToHide;

    void Awake()
    {
        instance = this;

        checkPointUIText = GetComponent<Text>();
        
        canShowCheckPointUI = false;
        timeToHide = 3f;
    }
    void Start()
    {
        CheckPoint.onPlayerTriggerEnter += OnPlayerTriggerEnter_CanShowCheckPointUI;

        checkPointUIText.color = new Color(checkPointUIText.color.r, checkPointUIText.color.g, checkPointUIText.color.b, 0f);
    }
    void Update()
    {
        if(canShowCheckPointUI)
        {
            ShowStaminaUIAnim();
        }
        else
        {
            CanHideStaminaUI();
            HideCheckPointUIAnim();
        }
    }

    private void ShowStaminaUIAnim()
    {
        Color alphaCheckPointText = checkPointUIText.color;

        if(alphaCheckPointText.a > 1f)
        {
            alphaCheckPointText.a = 1f;

            canShowCheckPointUI = false;

            timeToHide = 3f;
        }
        else
        {
            alphaCheckPointText.a += Time.deltaTime * 0.96f;
        }

        checkPointUIText.color = alphaCheckPointText;
    }
    private void HideCheckPointUIAnim()
    {
        Color alphaCheckPointText = checkPointUIText.color;

        if(alphaCheckPointText.a < 0f)
        {
            alphaCheckPointText.a = 0f;
        }
        else if(alphaCheckPointText.a > 0f && CanHideStaminaUI())
        {
            alphaCheckPointText.a -= Time.deltaTime * 0.96f;
        }

        checkPointUIText.color = alphaCheckPointText;
    }
    
    private void OnPlayerTriggerEnter_CanShowCheckPointUI(object sender, EventArgs e)
    {
        canShowCheckPointUI = true;
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
    
    public static void ShowCheckPointUIAnimStatic()
    {
        instance.canShowCheckPointUI = true;
    }

    void OnDisable()
    {
        checkPointUIText.color = new Color(checkPointUIText.color.r, checkPointUIText.color.g, checkPointUIText.color.b, 0f);
        CheckPoint.onPlayerTriggerEnter -= OnPlayerTriggerEnter_CanShowCheckPointUI;
        timeToHide = 0f;
    }
}
