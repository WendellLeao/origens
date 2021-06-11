using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollision : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyController enemyComponent;
    
    public void Damage()
    {
        enemyComponent.Damage();
    }

    public void Damage(int damageAmount)
    {
        enemyComponent.Damage(damageAmount);
    }
}
