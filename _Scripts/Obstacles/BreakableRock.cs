using System.Collections;
using UnityEngine;

public class BreakableRock : EnemyController
{
    [Header("Cracks")]
    [SerializeField] private GameObject cracksObj;
    [SerializeField] private Sprite[] cracksSprites;

    [Header("Particles")]
    [SerializeField] private ParticleSystem destructionParticle;

    [Header("Others")]
    [SerializeField] private GameObject breakableRockCollider;
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
        int cracksAmount = 3;
        float dividedHealth = healthSystem.GetHealthMax() / cracksAmount;

        if(healthSystem.GetHealth() <= dividedHealth * 2 && healthSystem.GetHealth() > dividedHealth)
        {
            cracksObj.GetComponent<SpriteRenderer>().sprite = cracksSprites[1];
        }
        else if(healthSystem.GetHealth() < dividedHealth)
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
            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            destructionParticle.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;

        if(GetComponent<PolygonCollider2D>() != null)
            GetComponent<PolygonCollider2D>().enabled = false;
        else
            GetComponent<BoxCollider2D>().enabled = false;

        cracksObj.SetActive(false);
        
        if(breakableRockCollider != null)
            breakableRockCollider.SetActive(false);

        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax);
        //Destroy(this.gameObject);
        Destroy(transform.parent.gameObject);
    }
}
