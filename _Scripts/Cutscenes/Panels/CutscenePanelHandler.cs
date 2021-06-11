using UnityEngine;

public class CutscenePanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private bool goToMenu = false;
    //[SerializeField] private GameObject nextDirector;
    
    void Start()
    {
        //Time.timeScale = 0f;
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     Time.timeScale = 1f;
        //     nextDirector.SetActive(true);
        //     Destroy(this.gameObject);
        // }        

        if(dialoguePanel == null)
        {
            if(goToMenu)
            {
                if(SceneHandler.instance != null)
                    SceneHandler.instance.LoadSceneWithTimer("MainMenu", 0.5f);
                else
                    Debug.LogError("Scene handler is null");
            }
            else
            {
                if(SceneHandler.instance != null)
                    SceneHandler.instance.LoadSceneWithTimer("LevelSelection", 0.5f);
                else
                    Debug.LogError("Scene handler is null");
            }
        }
    }
}
