using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionMenuHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject videoPanelObj;
    [SerializeField] private GameObject audioPanelObj;
    [SerializeField] private GameObject gamePanelObj;

    [Header("Other")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;
    private SwitchButtonsPanelScript switchButtons;
    private Resolution[] resolutions;

    void Awake()
    {
        if(GetComponent<SwitchButtonsPanelScript>() != null)
            switchButtons = GetComponent<SwitchButtonsPanelScript>();
    }
    
    void Start()
    {
        if(videoPanelObj != null)
        {
            ShowVideoPanel();
            switchButtons.OnButtonClicked_DisableItself(switchButtons.Buttons[0]);
        }

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        // resolutionDropdown.AddOptions(ResolutionHandler.instance.options);
        // resolutionDropdown.value = ResolutionHandler.instance.currentResolutionIndex;
        // resolutionDropdown.RefreshShownValue();
    }

    public void ShowVideoPanel()
    {
        videoPanelObj.SetActive(true);

        if(gamePanelObj != null)
            gamePanelObj.SetActive(false);
        
        if(audioPanelObj != null)
            audioPanelObj.SetActive(false);
    }
    public void ShowAudioPanel()
    {
        audioPanelObj.SetActive(true);

        if(gamePanelObj != null)
            gamePanelObj.SetActive(false);
        
        if(videoPanelObj != null)
            videoPanelObj.SetActive(false);
    }
    public void ShowGamePanel()
    {
        gamePanelObj.SetActive(true);
        
        if(videoPanelObj != null)
            videoPanelObj.SetActive(false);
        
        if(audioPanelObj != null)
            audioPanelObj.SetActive(false);
    }

    //Video options
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int reslutionIndex)
    {
        Resolution resolution = resolutions[reslutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // if(ResolutionHandler.instance != null)
        //     ResolutionHandler.instance.SetResolution(reslutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //Audio options
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    //Game options
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
