using UnityEngine;

public class PrimordialEnergyDataKeeper : MonoBehaviour
{
    public static PrimordialEnergyDataKeeper instance;
    [HideInInspector] public Transform[] primordialEnergiesPos;

    private bool canSpawnFirstPrimordialEnergy = true;
    private bool canSpawnSecondPrimordialEnergy = true;
    private bool canSpawnThirdPrimordialEnergy = true;

    void Awake()
    {
        primordialEnergiesPos[0] = PrimordialEnergyPositions.instance.primordialEnergiesPos[0];
        primordialEnergiesPos[1] = PrimordialEnergyPositions.instance.primordialEnergiesPos[1];
        primordialEnergiesPos[2] = PrimordialEnergyPositions.instance.primordialEnergiesPos[2];

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        primordialEnergiesPos[0] = PrimordialEnergyPositions.instance.primordialEnergiesPos[0];
        primordialEnergiesPos[1] = PrimordialEnergyPositions.instance.primordialEnergiesPos[1];
        primordialEnergiesPos[2] = PrimordialEnergyPositions.instance.primordialEnergiesPos[2];
    }

    public int canSpawnFirstPrimordialEnergyInt()
    {
        if (canSpawnFirstPrimordialEnergy)
            return 1;
        else
            return 0;
    }
    public int canSpawnSecondPrimordialEnergyInt()
    {
        if (canSpawnSecondPrimordialEnergy)
            return 1;
        else
            return 0;
    }
    public int canSpawnThirdPrimordialEnergyInt()
    {
        if (canSpawnThirdPrimordialEnergy)
            return 1;
        else
            return 0;
    }

    public bool CanSpawnFirstPrimordialEnergy
    {
        get{return canSpawnFirstPrimordialEnergy;}
        set{canSpawnFirstPrimordialEnergy = value;}
    }
    public bool CanSpawnSecondPrimordialEnergy
    {
        get{return canSpawnSecondPrimordialEnergy;}
        set{this.canSpawnSecondPrimordialEnergy = value;}
    }
    public bool CanSpawnThirdPrimordialEnergy
    {
        get{return canSpawnThirdPrimordialEnergy;}
        set{this.canSpawnThirdPrimordialEnergy = value;}
    }
}
