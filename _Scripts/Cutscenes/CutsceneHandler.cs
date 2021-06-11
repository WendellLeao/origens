using System;
using UnityEngine;
using UnityEngine.Playables;

//[RequireComponent(typeof(PlayableDirector))]
public class CutsceneHandler : MonoBehaviour
{
    public static CutsceneHandler instance;
    [SerializeField] private GameObject[] directors;
    [SerializeField] private GameObject pauseMenuObj;
    [HideInInspector] public Action onCutsceneComplete;
    private int index = 0;
    
    //[SerializeField] private Animator animatorPlayer;
    // private PlayableDirector director;
    // private RuntimeAnimatorController playerAnimatorController;
    // private bool playerAnimatorControllerWasFixed = false;
    private bool isPlayingCutscene = false;

    void Awake()
    {
        instance = this;

        //director = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        // playerAnimatorController = animatorPlayer.runtimeAnimatorController;
        // animatorPlayer.runtimeAnimatorController = null;
    }

    void Update()
    {
        // if(director.state != PlayState.Playing)
        // {
        //     animatorPlayer.runtimeAnimatorController = playerAnimatorController;
        //     playerAnimatorControllerWasFixed = true;
        //     //isPlayingCutscene = false;
        //     //OnCutsceneStop?.Invoke();
        // }
        // else
        // {
        //     isPlayingCutscene = true;
        // }
    }
    
    public void LoadNextDirector()
    {
        if(directors != null)
        {
            if(index <= directors.Length - 1)
            {
                directors[index].SetActive(true);
                index++;
            }
        }
    }

    public void SkipCutscene()
    {
        onCutsceneComplete?.Invoke();
    }

    public bool IsPlayingCutscene
    {
        get{return isPlayingCutscene;}
    }
}
