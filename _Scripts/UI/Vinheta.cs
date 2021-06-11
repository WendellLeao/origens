using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinheta : MonoBehaviour
{
    private bool canPlayAnim = false;
    void Update()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            if(canPlayAnim)
                GetComponent<AnimationHandler>().PlayAnimation("FadeOutAnim");
        }
        else
        {
            canPlayAnim = true;
            GetComponent<AnimationHandler>().PlayAnimation("FadeInAnim");
        }
    }
}
