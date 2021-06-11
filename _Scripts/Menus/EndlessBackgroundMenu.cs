using UnityEngine;
using System.Collections;

public class EndlessBackgroundMenu : MonoBehaviour
{
    [SerializeField] private float moveSpeedBG;

    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeedBG, 0, 0);
    }
}
