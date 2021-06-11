using System;
using UnityEngine;
public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetHealthMax()
    {
        return healthMax;
    }
    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }


    public void Damage()
    {
        health--;

        if(health < 0)
            health = 0;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
