using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionHandler : MonoBehaviour
{
    [Header("Level Buttons")]
    [SerializeField] private Button[] lvlButtons;

    [Header("Primordial Energy")]
    [SerializeField] private GameObject primordialEnergyHandlerPf;
    [SerializeField] private Text primordialEnergyTotalAmountText;
    [SerializeField] private int primordialEnergiesNeeded;

    void Awake()
    {
        if(PrimordialEnergyHandler.instance == null)
        {
            Instantiate(primordialEnergyHandlerPf, this.transform.position, this.transform.rotation);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        Fade.PlayFadeOut();

        int levelAt = PlayerPrefs.GetInt("levelAt", 2);

        for(int i = 0; i < lvlButtons.Length; i++)
        {
            if(i + 2 > levelAt)
            {
                lvlButtons[i].interactable = false;
            }
        }

        // if(PrimordialEnergyHandler.instance == null)
        //     Instantiate(primordialEnergyHandlerPf, this.transform.position, this.transform.rotation);
    }
    void Update()
    {
        if(PrimordialEnergyHandler.instance != null)
            primordialEnergyTotalAmountText.text = PlayerPrefs.GetInt("totalPrimordialEnergyAmount").ToString();

        if(Input.GetKeyDown(KeyCode.Escape))
            SceneHandler.instance.BackToMainMenu();
    }

    private void PlayTransition()
    {
        Fade.PlayFadeIn();

        if(!AudioHandler.instance.IsPlayingGameTheme)
            AudioHandler.instance.ChangeSoundtrackMusicWithTimer("MenuTheme", "GameTheme", 1.3f);
    }

    public void StartTutorial()
    {
        SceneHandler.instance.LoadSceneWithTimer("Level00", 1.3f);
        Fade.PlayFadeIn();

        AudioHandler.instance.IsPlayingGameTheme = false;
        AudioHandler.instance.Play("ButtonSoundFX");

        if(!AudioHandler.instance.IsPlayingMenuTheme)
        {
            AudioHandler.instance.Play("MenuTheme");
            AudioHandler.instance.StopPlaying("GameTheme");
            AudioHandler.instance.IsPlayingMenuTheme = true;
        }
        
        //PlayTransition();
    }
    
    public void StartLevel01()
    {
        SceneHandler.instance.LoadSceneWithTimer("Level01", 1.3f);
        PlayTransition();
    }
    public void StartLevel02()
    {
        SceneHandler.instance.LoadSceneWithTimer("03 Meeting Espectrum", 1.3f);
        //SceneHandler.instance.LoadSceneWithTimer("Level02", 1.3f);
        PlayTransition();
    }
    public void StartLevel03()
    {
        SceneHandler.instance.LoadSceneWithTimer("Level03", 1.3f);
        PlayTransition();
    }
    public void StartLevel04()
    {
        SceneHandler.instance.LoadSceneWithTimer("Level04", 1.3f);
        PlayTransition();
    }

    public void ChangeTheWorld()
    {
        if(PlayerPrefs.GetInt("totalPrimordialEnergyAmount") >= primordialEnergiesNeeded)
        {
            Debug.Log("Change World");

            int totalPrimordialEnergyAmount = PlayerPrefs.GetInt("totalPrimordialEnergyAmount");
            PlayerPrefs.SetInt("totalPrimordialEnergyAmount", totalPrimordialEnergyAmount - primordialEnergiesNeeded);
        }
        else
        {
            Debug.Log("Não possui estrelas o suficiente");
        }
    }
}
