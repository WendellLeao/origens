using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMaster : MonoBehaviour
{
    public static DebugMaster instance;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        //Debug.Log("build number: " + SceneManager.GetActiveScene().buildIndex);        
        Debug.Log("levelAt: " + PlayerPrefs.GetInt("levelAt"));        

        if(PrimordialEnergyHandler.instance != null)
        {
            if(Input.GetKeyDown(KeyCode.K))
                PrimordialEnergyHandler.instance.AddPrimordialEnergyInScene();
        }

        if(LevelWindowHandler.instance != null)
        {
            if(Input.GetKeyDown(KeyCode.F12))
                LevelWindowHandler.instance.ShowButtons();
        }

        if(Input.GetKeyDown(KeyCode.F8))
            PlayerPrefs.SetInt("levelAt", 7);          
    }
}
