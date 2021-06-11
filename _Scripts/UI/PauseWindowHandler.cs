using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindowHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenuObj;
    [SerializeField] private GameObject optionsMenuObj;
    [SerializeField] private GameObject goToLevelSelectionMenuObj;
    [SerializeField] private GameObject quitMenuObj;
    
    void Start()
    {
        optionsMenuObj.SetActive(false);
        goToLevelSelectionMenuObj.SetActive(false);
        quitMenuObj.SetActive(false);    
    }
    
    public void ResumeGame()
    {
        GameHandler.instance.ResumeGame();
    }
    public void RestartGame()
    {
        SceneHandler.instance.RestartGame();
    }
    public void BackToMainMenu()
    {
        SceneHandler.instance.BackToMainMenu();
    }
    public void BackToLevelSelection()
    {
        SceneHandler.instance.BackToLevelSelection();
    }

    public void ShowOptionsMenu()
    {
        optionsMenuObj.SetActive(true);

        goToLevelSelectionMenuObj.SetActive(false);
        pauseMenuObj.SetActive(false);
        quitMenuObj.SetActive(false);
    }
    public void HideOptionsMenu()
    {
        pauseMenuObj.SetActive(true);
        
        optionsMenuObj.SetActive(false);
        goToLevelSelectionMenuObj.SetActive(false);
        quitMenuObj.SetActive(false);
    }

    public void ShowGoToLevelSelectionMenu()
    {
        goToLevelSelectionMenuObj.SetActive(true);
        
        pauseMenuObj.SetActive(false);
        quitMenuObj.SetActive(false);
    }
    public void HideGoToLevelSelectionMenu()
    {
        pauseMenuObj.SetActive(true);
        
        goToLevelSelectionMenuObj.SetActive(false);
        quitMenuObj.SetActive(false);
    }

    public void ShowQuitMenu()
    {
        quitMenuObj.SetActive(true);
        
        pauseMenuObj.SetActive(false);
        goToLevelSelectionMenuObj.SetActive(false);
    }
    public void HideQuitMenu()
    {
        pauseMenuObj.SetActive(true);
        
        quitMenuObj.SetActive(false);
        goToLevelSelectionMenuObj.SetActive(false);
    }

    public void QuitGame()
    {
        SceneHandler.instance.isLoadingScene = true;

        Fade.PlayFadeIn();
        AudioHandler.instance.Play("ButtonSoundFX");
        StartCoroutine(QuitGameTimer());
    }

    IEnumerator QuitGameTimer()
    {
        yield return new WaitForSecondsRealtime(1.3f);
        Application.Quit();
    }
}
