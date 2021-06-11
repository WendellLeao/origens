using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableArea : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        DamageHandler damageablePlayer = other.GetComponent<DamageHandler>();
        
        if(damageablePlayer != null)
        {
            AudioHandler.instance.Play("PlayerBeHitedFX");
            BloquinhoBrancoController.instance.GetComponent<DamageHandler>().DamagePlayerPerUnit(damageAmount);
        }
    }
}
