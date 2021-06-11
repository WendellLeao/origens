using UnityEngine;
using UnityEngine.UI;

public class CanvasAssets : MonoBehaviour
{
    public static CanvasAssets instance;
    public GameObject vinhetaObj;
    public Image damageImage;
    public GameObject pauseWindowObj;
    public GameObject staminaBarObj;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        GetComponent<Canvas>().worldCamera = CamAssets.instance.canvasCam;
    }
}
