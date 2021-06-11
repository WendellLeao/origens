using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    AnimationHandler animationHandler;
    [SerializeField] private GameObject blackBackGroundObj;

    void Awake()
    {
        instance = this;
        
        animationHandler = GetComponent<AnimationHandler>();
        //blackBackGroundObj.SetActive(true);
    }
    public static void PlayFadeIn()
    {
        instance.animationHandler.PlayAnimation("FadeIn");
    }
    public static void PlayFadeOut()
    {
        instance.animationHandler.PlayAnimation("FadeOut");
    }
}
