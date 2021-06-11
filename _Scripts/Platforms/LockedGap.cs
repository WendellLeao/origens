using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedGap : MonoBehaviour
{
    public enum PrimordialEnergies{ firstPrimordialEnergyCollected, secondPrimordialEnergyCollected, thirdPrimordialEnergyCollected }
    [SerializeField] private PrimordialEnergies unlockGapWhen;
    [SerializeField] private ParticleSystem destructionParticle;
    private PrimordialEnergy firstPrimordialEnergy, secondPrimordialEnergy, thirdPrimordialEnergy;
    private bool canPlayParticle = true;
    private bool canCheckPE = false;

    void Update()
    {
        if(!canCheckPE)
            StartCoroutine(TimeToCheck());

        if(PrimordialEnergyHandler.instance != null)
        {
            if (PrimordialEnergyHandler.instance.firstPrimordialEnergyObj != null)
                firstPrimordialEnergy = PrimordialEnergyHandler.instance.firstPrimordialEnergyObj.GetComponent<PrimordialEnergy>();

            if (PrimordialEnergyHandler.instance.secondPrimordialEnergyObj != null)
                secondPrimordialEnergy = PrimordialEnergyHandler.instance.secondPrimordialEnergyObj.GetComponent<PrimordialEnergy>();

            if (PrimordialEnergyHandler.instance.thirdPrimordialEnergyObj != null)
                thirdPrimordialEnergy = PrimordialEnergyHandler.instance.thirdPrimordialEnergyObj.GetComponent<PrimordialEnergy>();

            if(canCheckPE)
            {
                UnlockGapHandler();
                DestroyOnAwakeHandler();
            }
        }
    }

    private void UnlockGapHandler()
    {
        if(unlockGapWhen == PrimordialEnergies.firstPrimordialEnergyCollected)
        {
            if(PrimordialEnergyHandler.instance.WasCollected_PE1)
                StartCoroutine(Break());
        }

        else if(unlockGapWhen == PrimordialEnergies.secondPrimordialEnergyCollected)
        {
            if(PrimordialEnergyHandler.instance.WasCollected_PE2)
                StartCoroutine(Break());
        }

        else if(unlockGapWhen == PrimordialEnergies.thirdPrimordialEnergyCollected)
        {
            if(PrimordialEnergyHandler.instance.WasCollected_PE3)
                StartCoroutine(Break());
        }
    }
    private void DestroyOnAwakeHandler()
    {
        if(PrimordialEnergyHandler.instance != null)
        {
            if(PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper != null)
            {
                if(unlockGapWhen == PrimordialEnergies.firstPrimordialEnergyCollected && !PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnFirstPrimordialEnergy)
                    Destroy(gameObject);

                else if(unlockGapWhen == PrimordialEnergies.secondPrimordialEnergyCollected && !PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnSecondPrimordialEnergy)
                    Destroy(gameObject);

                else if(unlockGapWhen == PrimordialEnergies.thirdPrimordialEnergyCollected && !PrimordialEnergyHandler.instance.CurrentPrimordialEnergyDataKeeper.CanSpawnThirdPrimordialEnergy)
                    Destroy(gameObject);
            }
        }
    }

    private IEnumerator Break()
    {
        yield return new WaitForSeconds(0.5f); 

        if(canPlayParticle)
        {
            destructionParticle.Play();

            AudioHandler.instance.Play("DestructionFX");
            
            canPlayParticle = false;
        }
        
        GetComponent<SpriteRenderer>().enabled = false;        
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax); 
        Destroy(this.gameObject);
    }
    private IEnumerator TimeToCheck()
    {
        yield return new WaitForSeconds(1f); 
        canCheckPE = true;
    }
}
