using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    [Header("Damage Player")]
    [SerializeField] private Fire fire;

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageHandler damageable = other.GetComponent<DamageHandler>();

        if(damageable != null)
            damageable.DamagePlayerPerSeconds(fire.FireForce);
            //damageable.DamagePlayerPerUnit(fire.FireForce);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        DamageHandler damageable = other.GetComponent<DamageHandler>();

        if(damageable != null)
            damageable.UnsubscribeDamagePlayerAmount(fire.FireForce);
            //damageable.DamagePlayerPerUnit(fire.FireForce);
    }
}
