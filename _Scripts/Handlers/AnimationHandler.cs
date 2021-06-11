using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private string currentAnimaton;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayAnimation(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);

        currentAnimaton = newAnimation;
    }

    public void PlayAnimationOnce(string newAnimation)
    {
        if (currentAnimaton != newAnimation)
            PlayAnimation(newAnimation);
    }
}
