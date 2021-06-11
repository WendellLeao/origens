using UnityEngine.UI;
using UnityEngine;

public class TelecineseTutorial : MonoBehaviour
{
    void Update()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            GetComponent<Text>().text = "Aperte a tecla Q para TROCAR DE ALMA";
        }
        else
        {
            GetComponent<Text>().text = 
                "Com o MOUSE, controle o BLOQUINHO PRETO.\n\nPressione e SEGURE o BOTÃO ESQUERDO\n do MOUSE para ativar a TELECINESE e PUXAR\n a PEDRA";
        }
    }
}
