using System;
using UnityEngine;
public class HealthSystemPlayer
{
    public event EventHandler OnHealthChanged;
    public float healthFloat;
    private float healthMaxFloat;

    public HealthSystemPlayer(float healthMaxFloat)
    {
        this.healthMaxFloat = healthMaxFloat;
        healthFloat = healthMaxFloat;
    }

    public float GetHealthFloat()
    {
        return healthFloat;
    }

    public float GetHealthMaxFloat()
    {
        return healthMaxFloat;
    }

    public float GetHealthPercent()
    {
        return healthFloat / healthMaxFloat;
    }

    public void PlayerDamagePerSeconds(float speedToDamage)
    {
        healthFloat -= speedToDamage * Time.deltaTime;
        
        if(healthFloat < 0f)
            healthFloat = 0f;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerDamage(float damageAmount)
    {
        healthFloat -= damageAmount;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerHeal()
    {
        healthFloat += Time.deltaTime * 18f;
        
        if(healthFloat > healthMaxFloat)
            healthFloat = healthMaxFloat;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
