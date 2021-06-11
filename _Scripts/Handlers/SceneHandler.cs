using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    [HideInInspector] public bool isLoadingScene = false;

    void Awake()
    {
        instance = this;
    }
    
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);

        if(CheckPointHandler.instance != null)
            Destroy(CheckPointHandler.instance.gameObject);

        isLoadingScene = true;
    }
    public void LoadSceneWithTimer(string scene, float time)
    {
        StartCoroutine(TimerToLoadScene(scene, time));
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if(PrimordialEnergyHandler.instance != null)
            PrimordialEnergyHandler.instance.ResetPrimordialEnergyAmountInScene();

        isLoadingScene = true;
    }
    public void ReloadSceneWithTimer(float time)
    {
        StartCoroutine(TimerToReloadScene(time));
    }
   
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if(CheckPointHandler.instance != null)
            Destroy(CheckPointHandler.instance.gameObject);

        isLoadingScene = true;
    }
    public void LoadNextSceneWithTimer(float time)
    {
        StartCoroutine(TimerToLoadNextScene(time));
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartGame()
    {
        ReloadSceneWithTimer(1.3f);
        Fade.PlayFadeIn();

        AudioHandler.instance.Play("ButtonSoundFX");

        if(PrimordialEnergyHandler.instance != null)
            PrimordialEnergyHandler.instance.ResetPrimordialEnergyAmountInScene();

        isLoadingScene = true;
    }
   
    public void BackToMainMenu()
    {
        LoadSceneWithTimer("MainMenu", 1.3f);

        Fade.PlayFadeIn();

        AudioHandler.instance.IsPlayingGameTheme = false;
        AudioHandler.instance.Play("ButtonSoundFX");

        if(PrimordialEnergyHandler.instance != null)
        {
            PrimordialEnergyHandler.instance.ResetPrimordialEnergyAmountSavedInScene();
            //PrimordialEnergyHandler.instance.LoadPrimordialEnergiesAgain();
        }

        isLoadingScene = true;
    }
    public void BackToLevelSelection()
    {
        LoadSceneWithTimer("LevelSelection", 1.3f);
        Fade.PlayFadeIn();

        AudioHandler.instance.Play("ButtonSoundFX");

        if(PrimordialEnergyHandler.instance != null)
        {
            PrimordialEnergyHandler.instance.ResetPrimordialEnergyAmountSavedInScene();
            //PrimordialEnergyHandler.instance.LoadPrimordialEnergiesAgain();
        }

        isLoadingScene = true;
    }
    IEnumerator TimerToLoadScene( string scene, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        LoadScene(scene);
    }
    IEnumerator TimerToReloadScene(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        ReloadScene();
    }
    IEnumerator TimerToLoadNextScene(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        LoadNextScene();
    }
}
