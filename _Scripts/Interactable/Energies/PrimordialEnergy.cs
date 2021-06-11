using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimordialEnergy : MonoBehaviour
{
    [SerializeField] private ParticleSystem primordialEnergyPS;
    [SerializeField] private SpriteRenderer diamondSprite;
    private string startSortingLayerName;
    private bool wasCollected = false;
    private bool canCollide = true;

    void Awake()
    {
        startSortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
    }

    void Update()
    {
        // if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        // {
        //     GetComponent<SpriteRenderer>().sortingLayerName = startSortingLayerName;
        //     diamondSprite.sortingLayerName = startSortingLayerName;

        //     GetComponent<SpriteRenderer>().sortingOrder = -2;
        //     diamondSprite.sortingOrder = -1;
        // }
        // else
        // {
        //     GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        //     diamondSprite.sortingLayerName = "Default";

        //     GetComponent<SpriteRenderer>().sortingOrder = 0;
        //     diamondSprite.sortingOrder = 1;
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && canCollide)
        {
            wasCollected = true;
            AudioHandler.instance.Play("PrimordialEnergyFX");
            PrimordialEnergyHandler.instance.AddPrimordialEnergyInScene();
            PrimordialEnergyUI.instance.CanShowPrimordialUI();
            //StaminaSystem.instance.MaximizeStamina();
            StartCoroutine(DestroyTimer());
            canCollide = false;
        }
    }

    private IEnumerator DestroyTimer()
    {
        primordialEnergyPS.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        diamondSprite.enabled = false;////////////////////////////////////
        
        yield return new WaitForSeconds(primordialEnergyPS.main.startLifetime.constantMax); 
        Destroy(this.gameObject);
    }

    public bool WasCollected
    {
        get{return wasCollected;}
    }
}
