using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPESpawner : MonoBehaviour
{
    public GameObject pe1, pe2, pe3;

    void Start()
    {
        pe1.SetActive(false);
        pe2.SetActive(false);
        pe3.SetActive(false);
    }
    void Update()
    {
        if(PrimordialEnergyHandler.instance != null && PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper != null)
        {
            if(PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy)
                pe1.SetActive(true);
            else
                pe1.SetActive(false);

            if(PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy)
                pe2.SetActive(true);
            else
                pe2.SetActive(false);

            if(PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy)
                pe3.SetActive(true);
            else
                pe3.SetActive(false);

            // if(PlayerPrefs.GetInt("canSpawnFirstPrimordialEnergy_Lvl1") == 1)
            //     pe1.SetActive(true);
            // else
            //     pe1.SetActive(false);

            // if(PlayerPrefs.GetInt("canSpawnSecondPrimordialEnergy_Lvl1") == 1)
            //     pe2.SetActive(true);
            // else
            //     pe2.SetActive(false);

            // if(PlayerPrefs.GetInt("canSpawnThirdPrimordialEnergy_Lvl1") == 1)
            //     pe3.SetActive(true);
            // else
            //     pe3.SetActive(false);
        }        
    }
}
