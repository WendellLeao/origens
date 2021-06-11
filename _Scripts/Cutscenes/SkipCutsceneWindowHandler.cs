using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipCutsceneWindowHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenuObj;
    [SerializeField] private GameObject skipMenuObj;

    public void ResumeCutscene()
    {
        GameHandler.instance.ResumeGame();
    }

    public void ShowSkipMenu()
    {
        skipMenuObj.SetActive(true);
        
        pauseMenuObj.SetActive(false);
        //goToLevelSelectionMenuObj.SetActive(false);

        AudioHandler.instance.Play("ButtonSoundFX");
    }
    public void HideSkipMenu()
    {
        pauseMenuObj.SetActive(true);
        
        skipMenuObj.SetActive(false);
        //goToLevelSelectionMenuObj.SetActive(false);

        AudioHandler.instance.Play("ButtonSoundFX");
    }

    public void SkipCutsceneEvent()
    {
        //SceneHandler.instance.BackToLevelSelection();
        if(CutsceneHandler.instance != null)
            CutsceneHandler.instance.SkipCutscene();

        AudioHandler.instance.Play("ButtonSoundFX");
    }
}
