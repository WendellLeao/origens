using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimordialEnergyPositions : MonoBehaviour
{
    public static PrimordialEnergyPositions instance;
 
    [Header("Positions")]
    public Transform[] primordialEnergiesPos;

    void Awake()
    {
        instance = this;
    }
}
