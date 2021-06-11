using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntireCommonEnergySpawner : EnemyController, IDamageable
{
    [Header("Spawn Broken")]
    [SerializeField] private GameObject brokenCommonEnergyPf;
    
    [Header("Cracks")]
    [SerializeField] private GameObject cracksObj;
    [SerializeField] private Sprite[] cracksSprites;

    [Header("Particles")]
    [SerializeField] private ParticleSystem destructionParticlePS;
    private bool canPlayParticleSystem = true;

    protected override void Start()
    {
        base.Start();
        
        cracksObj.GetComponent<SpriteRenderer>().sprite = cracksSprites[0];
    }

    protected override void Update()
    {
        CracksHandler();
        CheckHealth();
    }

    private void CracksHandler()
    {
        // TODO: Encontrar uma forma de calcular com base nos pontos de vida da pedra
        if(healthSystem.GetHealth() < 7 && healthSystem.GetHealth() > 4)
        {
            cracksObj.GetComponent<SpriteRenderer>().sprite = cracksSprites[1];
        }
        else if(healthSystem.GetHealth() < 4)
        {
            cracksObj.GetComponent<SpriteRenderer>().sprite = cracksSprites[2];
        }
    }

    private void CheckHealth()
    {
        if(healthSystem.GetHealth() <= 0)
            StartCoroutine(Break());
    }
    
    private IEnumerator Break()
    {
        if(canPlayParticleSystem)
        {
            Instantiate(brokenCommonEnergyPf, this.transform.position, this.transform.rotation);

            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            destructionParticlePS.Play();
            canPlayParticleSystem = false;
        }
            
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<EdgeCollider2D>().enabled = false;

        cracksObj.SetActive(false);

        yield return new WaitForSeconds(destructionParticlePS.main.startLifetime.constantMax); 
        Destroy(this.gameObject);
    }
}
