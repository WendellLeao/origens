using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWindowHandler : MonoBehaviour
{
    public static LevelWindowHandler instance;
    [SerializeField] private GameObject buttons;
    private bool showButtons = false;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(showButtons)
            buttons.SetActive(true);
        else
            buttons.SetActive(false);
    }

    public void ShowButtons()
    {
        showButtons = !showButtons;
    }

    public void Level00()
    {
        SceneHandler.instance.LoadScene("Level00");
    }
    public void Level01()
    {
        SceneHandler.instance.LoadScene("Level01");
    }
    public void Level02()
    {
        SceneHandler.instance.LoadScene("Level02");
    }
    public void Level03()
    {
        SceneHandler.instance.LoadScene("Level03");
    }
    public void Level04()
    {
        SceneHandler.instance.LoadScene("Level04");
    }
}
