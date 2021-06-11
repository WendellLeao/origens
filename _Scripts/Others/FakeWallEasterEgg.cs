using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWallEasterEgg : MonoBehaviour
{
    [SerializeField] private AnimationHandler animationHandler;
    private bool isShowed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        BloquinhoBrancoController bloquinhoBranco = other.GetComponent<BloquinhoBrancoController>();

        if(bloquinhoBranco != null)
            animationHandler.PlayAnimation("ShowAnim");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        BloquinhoBrancoController bloquinhoBranco = other.GetComponent<BloquinhoBrancoController>();

        if(bloquinhoBranco != null)
            animationHandler.PlayAnimation("HideAnim");
    }
}
