using System.Collections;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public enum Menu{ Entry, Main, Options, Controls, Quit }
    [SerializeField] private GameObject EntryMenuObj;
    [SerializeField] private GameObject MainMenuObj;
    [SerializeField] private GameObject OptionsMenuObj;
    [SerializeField] private GameObject ControlsMenuObj;
    [SerializeField] private GameObject QuitMenuObj;
    private Menu menu;

    void Start()
    {
        Time.timeScale = 1f;

        Fade.PlayFadeOut();

        EntryMenuObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        MainMenuObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        OptionsMenuObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ControlsMenuObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        QuitMenuObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        ShowMenu(Menu.Entry);
        menu = Menu.Entry;

        if(!AudioHandler.instance.IsPlayingMenuTheme)
        {
            AudioHandler.instance.Play("MenuTheme");
            AudioHandler.instance.StopPlaying("GameTheme");
            AudioHandler.instance.IsPlayingMenuTheme = true;
        }
    }

    void Update()
    {
        if(menu == Menu.Entry && Input.anyKey)
        {
            AudioHandler.instance.Play("ButtonSoundFX");
            ShowMenu(Menu.Main);
            menu = Menu.Main;
        }
        
        if(menu == Menu.Controls || menu == Menu.Options || menu == Menu.Quit)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                BackToMenu();
        }
    }
     
    public void ShowMenu(Menu menu)
    {
        EntryMenuObj.SetActive(false);
        MainMenuObj.SetActive(false);
        OptionsMenuObj.SetActive(false);
        ControlsMenuObj.SetActive(false);
        QuitMenuObj.SetActive(false);

        switch(menu)
        {
            case Menu.Entry:
                EntryMenuObj.SetActive(true);
                break; 
            case Menu.Main:
                MainMenuObj.SetActive(true);
                break;
            case Menu.Options:
                OptionsMenuObj.SetActive(true);
                break;
            case Menu.Controls:
                ControlsMenuObj.SetActive(true);
                break;
            case Menu.Quit:
                QuitMenuObj.SetActive(true);
                break;
        }
    }

    public void PlayGame()
    {
        Fade.PlayFadeIn();

        AudioHandler.instance.Play("ButtonSoundFX");

        // if(PlayerPrefs.GetInt("levelAt") < 2)
        //     SceneHandler.instance.LoadSceneWithTimer("Level00", 1.3f);
        // else
        //     SceneHandler.instance.LoadSceneWithTimer("LevelSelection", 1.3f);
        if(PlayerPrefs.GetInt("levelAt") < 2)
            SceneHandler.instance.LoadSceneWithTimer("01 FirstCutscene", 1.3f);
        else
            SceneHandler.instance.LoadSceneWithTimer("LevelSelection", 1.3f);
    }

    public void ShowControlsMenu()
    {
        AudioHandler.instance.Play("ButtonSoundFX");
        ShowMenu(Menu.Controls);
        menu = Menu.Controls;
    }

    public void ShowOptionsMenu()
    {
        AudioHandler.instance.Play("ButtonSoundFX");
        ShowMenu(Menu.Options);
        menu = Menu.Options;
    }

    public void BackToMenu()
    {
        AudioHandler.instance.Play("ButtonSoundFX");
        ShowMenu(Menu.Main);
        menu = Menu.Main;
    }

    public void ShowQuitMenu()
    {
        AudioHandler.instance.Play("ButtonSoundFX");
        ShowMenu(Menu.Quit);
        menu = Menu.Quit;
    }
    public void Quit()
    {
        SceneHandler.instance.isLoadingScene = true;

        Fade.PlayFadeIn();
        AudioHandler.instance.Play("ButtonSoundFX");
        StartCoroutine(QuitTimer());
    }

    IEnumerator QuitTimer()
    {
        yield return new WaitForSeconds(1.3f);
        Application.Quit();
    }
}
