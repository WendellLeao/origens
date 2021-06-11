using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PrimordialEnergyHandler : MonoBehaviour
{
    public static PrimordialEnergyHandler instance;

    [Header("Primordial Energy Object")]
    [SerializeField] private GameObject primordialEnergyPf;
    [SerializeField] private GameObject collectedPrimordialEnergyPf;
    [HideInInspector] public GameObject firstPrimordialEnergyObj, secondPrimordialEnergyObj, thirdPrimordialEnergyObj;
    [SerializeField] private GameObject primordialEnergyDataKeeperPf;

    [Header("Handler")]
    [SerializeField] private PrimordialEnergyDataKeeper[] keepers;
    private PrimordialEnergyDataKeeper currentPrimordialEnergyDataKeeper;
    private int primordialEnergyAmountInScene;
    private int primordialEnergyAmountSavedInScene;
    private bool canCount;
    private bool canSpawnPrimordialEnergy;
    private bool canSavePrimordialEnergy = false;

    [SerializeField] private bool wasCollected_PE1 = true;
    [SerializeField] private bool wasCollected_PE2 = true;
    [SerializeField] private bool wasCollected_PE3 = true;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {        
        CheckPoint.onPlayerTriggerEnter += OnPlayerTriggerEnter_SavePrimordialEnergyAmount;   
        CheckPoint.onPlayerExitTriggerEnter += OnPlayerExitTriggerEnter_CanSavePrimordialEnergy; 
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "01 FirstCutscene") //if(SceneManager.GetActiveScene().name == "Level00")
            ResetPrimordialEnergiesToDefault();

        if(SceneManager.GetActiveScene().name == "LevelSelection")
            ReloadPrimordialEnergies();

        if(SceneManager.GetActiveScene().name != "Level00")
        {
            DataKeepersHandler();
            
            if(Input.GetKeyDown(KeyCode.S))
            canSavePrimordialEnergy = true;
            
            if(firstPrimordialEnergyObj != null)
                wasCollected_PE1 = firstPrimordialEnergyObj.GetComponent<PrimordialEnergy>().WasCollected;

            if(secondPrimordialEnergyObj != null)
                wasCollected_PE2 = secondPrimordialEnergyObj.GetComponent<PrimordialEnergy>().WasCollected;

            if(thirdPrimordialEnergyObj != null)
                wasCollected_PE3 = thirdPrimordialEnergyObj.GetComponent<PrimordialEnergy>().WasCollected;
        }
    }

    private void DataKeepersHandler()
    {
        if(keepers != null)
        {
            if(SceneManager.GetActiveScene().name == "Level01" && keepers[0] == null)
            {
                GameObject primordialEnergyDataKeeper01Obj = Instantiate(primordialEnergyDataKeeperPf, this.transform.position, this.transform.rotation);
                PrimordialEnergyHandler.instance.keepers[0] = primordialEnergyDataKeeper01Obj.GetComponent<PrimordialEnergyDataKeeper>();

                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl1", primordialEnergyDataKeeper01Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl1", primordialEnergyDataKeeper01Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl1", primordialEnergyDataKeeper01Obj.GetComponent<PrimordialEnergyDataKeeper>());
            }
            if(SceneManager.GetActiveScene().name == "Level02" && keepers[1] == null)
            {
                GameObject primordialEnergyDataKeeper02Obj = Instantiate(primordialEnergyDataKeeperPf, this.transform.position, this.transform.rotation);
                PrimordialEnergyHandler.instance.keepers[1] = primordialEnergyDataKeeper02Obj.GetComponent<PrimordialEnergyDataKeeper>();

                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl2", primordialEnergyDataKeeper02Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl2", primordialEnergyDataKeeper02Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl2", primordialEnergyDataKeeper02Obj.GetComponent<PrimordialEnergyDataKeeper>());
            }
            if(SceneManager.GetActiveScene().name == "Level03" && keepers[2] == null)
            {
                GameObject primordialEnergyDataKeeper03Obj = Instantiate(primordialEnergyDataKeeperPf, this.transform.position, this.transform.rotation);
                PrimordialEnergyHandler.instance.keepers[2] = primordialEnergyDataKeeper03Obj.GetComponent<PrimordialEnergyDataKeeper>();

                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl3", primordialEnergyDataKeeper03Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl3", primordialEnergyDataKeeper03Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl3", primordialEnergyDataKeeper03Obj.GetComponent<PrimordialEnergyDataKeeper>());
            }
            if(SceneManager.GetActiveScene().name == "Level04" && keepers[3] == null)
            {
                GameObject primordialEnergyDataKeeper04Obj = Instantiate(primordialEnergyDataKeeperPf, this.transform.position, this.transform.rotation);
                PrimordialEnergyHandler.instance.keepers[3] = primordialEnergyDataKeeper04Obj.GetComponent<PrimordialEnergyDataKeeper>();

                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl4", primordialEnergyDataKeeper04Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl4", primordialEnergyDataKeeper04Obj.GetComponent<PrimordialEnergyDataKeeper>());
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl4", primordialEnergyDataKeeper04Obj.GetComponent<PrimordialEnergyDataKeeper>());
            }
        }

        SetCurrentDataKeeper();
    }

    private void LoadFirstPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if(PlayerPrefs.GetInt(key) == 1)
            dataKeeper.CanSpawnFirstPrimordialEnergy = true;
        else
            dataKeeper.CanSpawnFirstPrimordialEnergy = false;
    }
    private void LoadSecondPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if (PlayerPrefs.GetInt(key) == 1)
            dataKeeper.CanSpawnSecondPrimordialEnergy = true;
        else
            dataKeeper.CanSpawnSecondPrimordialEnergy = false;
    }
    private void LoadThirdPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if (PlayerPrefs.GetInt(key) == 1)
            dataKeeper.CanSpawnThirdPrimordialEnergy = true;
        else
            dataKeeper.CanSpawnThirdPrimordialEnergy = false;
    }

    private void SaveFirstPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if(dataKeeper.CanSpawnFirstPrimordialEnergy)
            PlayerPrefs.SetInt(key, 1);
        else
            PlayerPrefs.SetInt(key, 0);
    }
    private void SaveSecondPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if(dataKeeper.CanSpawnSecondPrimordialEnergy)
            PlayerPrefs.SetInt(key, 1);
        else
            PlayerPrefs.SetInt(key, 0);
    }
    private void SaveThirdPE_PlayerPrefs(string key, PrimordialEnergyDataKeeper dataKeeper)
    {
        if(dataKeeper.CanSpawnThirdPrimordialEnergy)
            PlayerPrefs.SetInt(key, 1);
        else
            PlayerPrefs.SetInt(key, 0);
    }

    public void ResetPrimordialEnergiesToDefault()
    {
        if(keepers != null)
        {
            for(int i = 0; i <= 3; i++)
            {
                keepers[i] = null;
            }
        }

        PlayerPrefs.SetInt("canSpawnFirstPrimordialEnergy_Lvl1", 1);
        PlayerPrefs.SetInt("canSpawnSecondPrimordialEnergy_Lvl1", 1);
        PlayerPrefs.SetInt("canSpawnThirdPrimordialEnergy_Lvl1", 1);

        PlayerPrefs.SetInt("canSpawnFirstPrimordialEnergy_Lvl2", 1);
        PlayerPrefs.SetInt("canSpawnSecondPrimordialEnergy_Lvl2", 1);
        PlayerPrefs.SetInt("canSpawnThirdPrimordialEnergy_Lvl2", 1);

        PlayerPrefs.SetInt("canSpawnFirstPrimordialEnergy_Lvl3", 1);
        PlayerPrefs.SetInt("canSpawnSecondPrimordialEnergy_Lvl3", 1);
        PlayerPrefs.SetInt("canSpawnThirdPrimordialEnergy_Lvl3", 1);

        PlayerPrefs.SetInt("canSpawnFirstPrimordialEnergy_Lvl4", 1);
        PlayerPrefs.SetInt("canSpawnSecondPrimordialEnergy_Lvl4", 1);
        PlayerPrefs.SetInt("canSpawnThirdPrimordialEnergy_Lvl4", 1);
    } 
    public void ReloadPrimordialEnergies()
    {
        if(keepers != null && SceneManager.GetActiveScene().name != "Level00")
        {
            if(keepers[0] != null)
            {
                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl1", keepers[0]);
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl1", keepers[0]);
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl1", keepers[0]);
            }
            if(keepers[1] != null)
            {
                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl2", keepers[1]);
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl2", keepers[1]);
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl2", keepers[1]);
            }
            if(keepers[2] != null)
            {
                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl3", keepers[2]);
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl3", keepers[2]);
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl3", keepers[2]);
            }
            if(keepers[3] != null)
            {
                LoadFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl4", keepers[3]);
                LoadSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl4", keepers[3]);
                LoadThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl4", keepers[3]);
            }
        }
    }

    public void SetCollectedPrimordialEnergies()
    {
        if(currentPrimordialEnergyDataKeeper != null && SceneManager.GetActiveScene().name != "Level00")
        {
            if(!currentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy)
                wasCollected_PE1 = true;
            else
                wasCollected_PE1 = false;

            if(!currentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy)
                wasCollected_PE2 = true;
            else
                wasCollected_PE2 = false;

            if(!currentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy)
                wasCollected_PE3 = true;
            else
                wasCollected_PE3 = false;
        }
    }
    
    public void SetCurrentDataKeeper()
    {
        if(keepers != null && SceneManager.GetActiveScene().name != "Level00")
        {
            if(SceneManager.GetActiveScene().name == "Level01")
            {
                currentPrimordialEnergyDataKeeper = PrimordialEnergyHandler.instance.keepers[0];
            }
            if(SceneManager.GetActiveScene().name == "Level02")
            {
                currentPrimordialEnergyDataKeeper = PrimordialEnergyHandler.instance.keepers[1];
            }
            if(SceneManager.GetActiveScene().name == "Level03")
            {
                currentPrimordialEnergyDataKeeper = PrimordialEnergyHandler.instance.keepers[2];
            }
            if(SceneManager.GetActiveScene().name == "Level04")
            {
                currentPrimordialEnergyDataKeeper = PrimordialEnergyHandler.instance.keepers[3];
            }
        }
    }
    
    public void SpawnPrimordialEnergies()
    {
        if(canSpawnPrimordialEnergy && SceneManager.GetActiveScene().name != "Level00")
        {
            if(currentPrimordialEnergyDataKeeper != null)
            {
                if(currentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy && currentPrimordialEnergyDataKeeper.primordialEnergiesPos[0] != null)
                {
                    firstPrimordialEnergyObj = Instantiate(primordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[0].transform.position, this.transform.rotation);
                    firstPrimordialEnergyObj.name = "firstPrimordialEnergy(Clone)";
                }
                else
                {
                    Instantiate(collectedPrimordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[0].transform.position, this.transform.rotation);
                }

                if (currentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy && currentPrimordialEnergyDataKeeper.primordialEnergiesPos[1] != null)
                {
                    secondPrimordialEnergyObj = Instantiate(primordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[1].transform.position, this.transform.rotation);
                    secondPrimordialEnergyObj.name = "secondPrimordialEnergy(Clone)";
                }
                else
                {
                    Instantiate(collectedPrimordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[1].transform.position, this.transform.rotation);
                }

                if (currentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy && currentPrimordialEnergyDataKeeper.primordialEnergiesPos[2] != null)
                {
                    thirdPrimordialEnergyObj = Instantiate(primordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[2].transform.position, this.transform.rotation);
                    thirdPrimordialEnergyObj.name = "thirdPrimordialEnergyObj(Clone)";
                }                
                else
                {
                    Instantiate(collectedPrimordialEnergyPf, currentPrimordialEnergyDataKeeper.primordialEnergiesPos[2].transform.position, this.transform.rotation);
                }
            }

            canSpawnPrimordialEnergy = false;
        }
    }

    private void OnPlayerTriggerEnter_SavePrimordialEnergyAmount(object sender, EventArgs e)
    {
        if(SceneManager.GetActiveScene().name != "Level00")
        {
            SavePrimordialEnergyInScene();

            if(currentPrimordialEnergyDataKeeper != null && canSavePrimordialEnergy)
            {
                if(wasCollected_PE1)
                    currentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy = false;
                else
                    currentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy = true;

                if(wasCollected_PE2)
                    currentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy = false;
                else
                    currentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy = true;

                if(wasCollected_PE3)
                    currentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy = false;
                else
                    currentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy = true;
            }
        }
    }

    private void OnPlayerExitTriggerEnter_CanSavePrimordialEnergy(object sender, EventArgs e)
    {
        if(SceneManager.GetActiveScene().name != "Level00")
            canSavePrimordialEnergy = true;
    }

    public void UpdatePrimordialEnergyTotalAmount()
    {
        SavePrimordialEnergyInScene();

        if(canCount)
        {
            int totalPrimordialEnergyAmount = PlayerPrefs.GetInt("totalPrimordialEnergyAmount");
            PlayerPrefs.SetInt("totalPrimordialEnergyAmount", totalPrimordialEnergyAmount + primordialEnergyAmountSavedInScene);
            canCount = false;
        }

        if(currentPrimordialEnergyDataKeeper != null)
        {
            if(firstPrimordialEnergyObj == null)
                currentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy = false;

            if(secondPrimordialEnergyObj == null)
                currentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy = false;

            if(thirdPrimordialEnergyObj == null)
                currentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy = false;
        }

        if(keepers != null)
        {
            if(keepers[0] != null)
            {
                SaveFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl1", keepers[0].GetComponent<PrimordialEnergyDataKeeper>());
                SaveSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl1", keepers[0].GetComponent<PrimordialEnergyDataKeeper>());
                SaveThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl1", keepers[0].GetComponent<PrimordialEnergyDataKeeper>());
            }

            if(keepers[1] != null)
            {
                SaveFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl2", keepers[1].GetComponent<PrimordialEnergyDataKeeper>());
                SaveSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl2", keepers[1].GetComponent<PrimordialEnergyDataKeeper>());
                SaveThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl2", keepers[1].GetComponent<PrimordialEnergyDataKeeper>());
            }

            if(keepers[2] != null)
            {
                SaveFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl3", keepers[2].GetComponent<PrimordialEnergyDataKeeper>());
                SaveSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl3", keepers[2].GetComponent<PrimordialEnergyDataKeeper>());
                SaveThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl3", keepers[2].GetComponent<PrimordialEnergyDataKeeper>());
            }

            if(keepers[3] != null)
            {
                SaveFirstPE_PlayerPrefs("canSpawnFirstPrimordialEnergy_Lvl4", keepers[3].GetComponent<PrimordialEnergyDataKeeper>());
                SaveSecondPE_PlayerPrefs("canSpawnSecondPrimordialEnergy_Lvl4", keepers[3].GetComponent<PrimordialEnergyDataKeeper>());
                SaveThirdPE_PlayerPrefs("canSpawnThirdPrimordialEnergy_Lvl4", keepers[3].GetComponent<PrimordialEnergyDataKeeper>());
            }
        }
    }

    public void AddPrimordialEnergyInScene()
    {
        primordialEnergyAmountInScene++;
    }
    
    private void SavePrimordialEnergyInScene()
    {
        primordialEnergyAmountSavedInScene = primordialEnergyAmountInScene;
    }

    public void ResetPrimordialEnergyAmountInScene()
    {
        primordialEnergyAmountInScene = primordialEnergyAmountSavedInScene;
    }
    public void ResetPrimordialEnergyAmountSavedInScene()
    {
        primordialEnergyAmountSavedInScene = 0;
        ResetPrimordialEnergyAmountInScene();
    }

    public PrimordialEnergyDataKeeper CurrentPrimordialEnergyDataKeeper
    {
        get{return currentPrimordialEnergyDataKeeper;}
    }
    public int PrimordialEnergyAmountInScene
    {
        get{return primordialEnergyAmountInScene;}
    }
    public bool CanCount
    {
        get{return canCount;}
        set{this.canCount = value;}
    }
    public bool CanSpawnPrimordialEnergy
    {
        get{return canSpawnPrimordialEnergy;}
        set{this.canSpawnPrimordialEnergy = value;}
    }
    public bool CanSavePrimordialEnergy
    {
        get{return canSavePrimordialEnergy;}
        set{this.canSavePrimordialEnergy = value;}
    }
    public bool WasCollected_PE1
    {
        get{return wasCollected_PE1;}
    }
    public bool WasCollected_PE2
    {
        get{return wasCollected_PE2;}
    }
    public bool WasCollected_PE3
    {
        get{return wasCollected_PE3;}
    }

    void OnDisable()
    {
        CheckPoint.onPlayerTriggerEnter -= OnPlayerTriggerEnter_SavePrimordialEnergyAmount;
        CheckPoint.onPlayerExitTriggerEnter -= OnPlayerExitTriggerEnter_CanSavePrimordialEnergy; 
    }
}
