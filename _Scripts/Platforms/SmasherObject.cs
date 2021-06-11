using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherObject : MonoBehaviour
{
    [SerializeField] private WayPointSystem wayPointSystem;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(wayPointSystem != null && wayPointSystem.IsMovingDown())
        {
            //Damage enemy
            IDamageable damageable = other.GetComponent<IDamageable>();

            if(damageable != null)
                damageable.Damage(100);

            //Damage player
            DamageHandler damageHandler = other.GetComponent<DamageHandler>();

            if (damageHandler != null && BloquinhoBrancoController.instance.IsGrounded())
            {
                DamageHandler.instance.SetDamageImageAlphaValue(200f);
                DamageHandler.instance.DamagePlayerPerUnit(300);
            }
        }
    }
}
