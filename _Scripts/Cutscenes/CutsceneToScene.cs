using UnityEngine;

public class CutsceneToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneHandler.instance.LoadSceneWithTimer(sceneName, 1.3f);
    }

    public void LoadSceneButtonEvent()
    {
        Fade.PlayFadeIn();
        AudioHandler.instance.Play("ButtonSoundFX");
        SceneHandler.instance.LoadSceneWithTimer(sceneName, 1.3f);
    }
}
