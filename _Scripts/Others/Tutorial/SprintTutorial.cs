using UnityEngine;
using UnityEngine.UI;

public class SprintTutorial : MonoBehaviour
{
    void Update()
    {
        if(StaminaSystem.instance.GetStaminaAmount() != 0f)
            GetComponent<Text>().text = "Com a STAMINA cheia, \nSEGURE SHIFT+D para CORRER e PULE";
        else
            GetComponent<Text>().text = "ABSORVA as energias para encher a STAMINA";
    }
}
