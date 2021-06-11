using Cinemachine;
using UnityEngine;

public class CamAssets : MonoBehaviour
{
    public static CamAssets instance;

    public CinemachineVirtualCamera virtualCameraBB;
    public CinemachineVirtualCamera virtualCameraBP;
    public CinemachineBrain cinemachineBrain;
    public Camera canvasCam;

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
}
