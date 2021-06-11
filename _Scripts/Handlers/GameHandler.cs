using UnityEngine;
using UnityEngine.SceneManagement;
public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    [Header("Primordial Energy")]
    [SerializeField] private GameObject primordialEnergyHandlerPf;
    [SerializeField] private GameObject audioHandlerPf;

    [Header("Checkpoint Handler")]
    [SerializeField] private bool spawnCheckpointHandler;
    [SerializeField] private GameObject checkPointHandlerPf;
    
    [Header("Debug Master")]
    [SerializeField] private bool spawnDebugMaster;
    [SerializeField] private GameObject debugMasterPf;

    
    [Header("Level Selection")]
    private int nextSceneLoad;
    
    void Awake()
    {
        instance = this;
        
        InstantiateHandlers();
    }
    void Start()
    {
        if(DamageHandler.instance != null)
            DamageHandler.instance.onPlayerDied += PlayerDied;
        
        if(BloquinhoBrancoController.instance != null)
            BloquinhoBrancoController.instance.onLevelComplete += LevelComplete;

        if(CutsceneHandler.instance != null)
            CutsceneHandler.instance.onCutsceneComplete += LevelComplete;

        CanvasAssets.instance.pauseWindowObj.SetActive(false);
        Time.timeScale = 1f;

        nextSceneLoad = SceneManager.GetActiveScene().buildIndex; /////////////////////

        if(PrimordialEnergyHandler.instance != null)
        {
            PrimordialEnergyHandler.instance.SetCollectedPrimordialEnergies();
            PrimordialEnergyHandler.instance.SetCurrentDataKeeper();

            PrimordialEnergyHandler.instance.CanSpawnPrimordialEnergy = true;
            PrimordialEnergyHandler.instance.CanSavePrimordialEnergy = false;
            PrimordialEnergyHandler.instance.CanCount = true;

            if(SceneManager.GetActiveScene().name == "Level00") 
                Destroy(PrimordialEnergyHandler.instance.gameObject);
        }

        Fade.PlayFadeOut();
    }
    void Update()
    {
        ////////
        //Debug.Log(nextSceneLoad);

        if(Input.GetKeyDown(KeyCode.Escape) && !SceneHandler.instance.isLoadingScene) // && CutsceneHandler.instance == null
        {
            if(IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if(Input.GetKey(KeyCode.Tab) && Time.timeScale == 1f && !SceneHandler.instance.isLoadingScene)
        {
            if(PrimordialEnergyUI.instance != null)
                PrimordialEnergyUI.instance.CanShowPrimordialUI();
            
            if(StaminaSystem.instance != null)
                StaminaBar.ShowStaminaUIAnimStatic();
        }

        if(PrimordialEnergyHandler.instance != null && SceneManager.GetActiveScene().name != "Level00")
            PrimordialEnergyHandler.instance.SpawnPrimordialEnergies();
    }

    private void InstantiateHandlers()
    {
        if(CheckPointHandler.instance == null && spawnCheckpointHandler)
        {
            Instantiate(checkPointHandlerPf, this.transform.position, this.transform.rotation);
        }

        if(AudioHandler.instance == null)
        {
            Instantiate(audioHandlerPf, this.transform.position, this.transform.rotation);

            //AudioHandler.instance.Play("GameTheme");   
        }

        if(PrimordialEnergyHandler.instance == null)
        {
            Instantiate(primordialEnergyHandlerPf, this.transform.position, this.transform.rotation);
        }

        if(DebugMaster.instance == null && debugMasterPf != null && spawnDebugMaster)
        {
            Instantiate(debugMasterPf, this.transform.position, this.transform.rotation);
        }
    }
    
    private void PlayerDied()
    {
        Fade.PlayFadeIn();
        
        SceneHandler.instance.ReloadSceneWithTimer(1.3f);
        SceneHandler.instance.isLoadingScene = true;
    }

    public void LevelComplete()
    {
        Fade.PlayFadeIn();

        if(SceneManager.GetActiveScene().name != "Level00" && SceneManager.GetActiveScene().name != "Level04")
            SceneHandler.instance.LoadSceneWithTimer("LevelSelection", 1.3f);
        else if(SceneManager.GetActiveScene().name == "Level00")
            SceneHandler.instance.LoadSceneWithTimer("02 Finding Bloquinho Preto", 1.3f);
        else if(SceneManager.GetActiveScene().name == "Level04")
            SceneHandler.instance.LoadSceneWithTimer("04 Transition To Next Age", 1.3f);

        SceneHandler.instance.isLoadingScene = true;

        if(SceneManager.GetActiveScene().name != "02 Finding Bloquinho Preto" && SceneManager.GetActiveScene().name != "03 Meeting Espectrum")
        {
            if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))/////////////////
            {
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
            }
        }
        
        if(PrimordialEnergyHandler.instance != null && SceneManager.GetActiveScene().name != "Level00")
        {
            PrimordialEnergyHandler.instance.UpdatePrimordialEnergyTotalAmount();
            PrimordialEnergyHandler.instance.ResetPrimordialEnergyAmountSavedInScene();
        }
    }

    public void ResumeGame()
    {
        AudioHandler.instance.Play("ButtonSoundFX");
        CanvasAssets.instance.pauseWindowObj.SetActive(false);
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        AudioHandler.instance.Play("ButtonSoundFX");

        if(CursorHandler.instance != null)
            CursorHandler.instance.ShowCursor();
        
        CanvasAssets.instance.pauseWindowObj.SetActive(true);

        if(CutsceneHandler.instance == null)
        {
            CanvasAssets.instance.pauseWindowObj.GetComponent<PauseWindowHandler>().HideQuitMenu();
            CanvasAssets.instance.pauseWindowObj.GetComponent<PauseWindowHandler>().HideOptionsMenu();
            CanvasAssets.instance.pauseWindowObj.GetComponent<PauseWindowHandler>().HideGoToLevelSelectionMenu();
        }
        else
        {
            CanvasAssets.instance.pauseWindowObj.GetComponent<SkipCutsceneWindowHandler>().HideSkipMenu();
        }
        
        Time.timeScale = 0f;
    }

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }

    void OnDisable()
    {
        if(DamageHandler.instance != null)
            DamageHandler.instance.onPlayerDied -= PlayerDied;

        if(BloquinhoBrancoController.instance != null)
            BloquinhoBrancoController.instance.onLevelComplete -= LevelComplete;
        
        if(CutsceneHandler.instance != null)
            CutsceneHandler.instance.onCutsceneComplete -= LevelComplete;
    }
}
