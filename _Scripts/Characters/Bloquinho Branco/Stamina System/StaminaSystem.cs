using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StaminaSystem
{
    public static StaminaSystem instance;
    [SerializeField] private float staminaAmount;
    [SerializeField] private int maxStamina;
    public event EventHandler onStaminaChanged;
    public event EventHandler onStaminaMaximized;

    public StaminaSystem(int maxStamina)
    {
        instance = this;

        this.maxStamina = maxStamina;
        staminaAmount = 0f; //maxStamina
    }

    public void DecreaseStamina()
    {
        onStaminaChanged?.Invoke(this, EventArgs.Empty);
       
        staminaAmount -= Time.deltaTime * 4f;
            
        if(staminaAmount < 0f)
            staminaAmount = 0f;
    }

    public void DecreaseStamina(float staminaAmount)
    {
        onStaminaChanged?.Invoke(this, EventArgs.Empty);
        this.staminaAmount -= staminaAmount;
    }

    public void AddStamina()
    {
        onStaminaChanged?.Invoke(this, EventArgs.Empty);

        if(staminaAmount < maxStamina)
        {
            staminaAmount += 1f;
        }

        else if(staminaAmount >= maxStamina)
        {
            staminaAmount = maxStamina;
        }
    }

    public void AddStaminaWithoutChangeStaminaBar()
    {
        if(staminaAmount < maxStamina)
        {
            staminaAmount += 1f;
        }

        else if(staminaAmount >= maxStamina)
        {
            staminaAmount = maxStamina;
        }
    }

    public void InvokeOnStaminaChanged()
    {
        onStaminaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void MaximizeStamina()
    {
        onStaminaMaximized?.Invoke(this, EventArgs.Empty);
        onStaminaChanged?.Invoke(this, EventArgs.Empty);
        staminaAmount = maxStamina;
    }

    public void SetStaminaAmount(float staminaAmount)
    {
        this.staminaAmount = staminaAmount;
    }

    public float GetStaminaAmount()
    {
        return staminaAmount;
    }

    public float GetStaminaNormalized()
    {
        return staminaAmount / maxStamina;
    }

    public float GetMaxStamina()
    {
        return this.maxStamina;
    }
}
