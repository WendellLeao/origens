using System.Collections;
using UnityEngine;

public class Stalactite : EnemyController
{
    [Header("Point")]
    [SerializeField] private GameObject stalactiteTipPf;

    [Header("Cracks")]
    [SerializeField] private GameObject cracksObj;
    [SerializeField] private Sprite baseStalactiteSprite;
    [SerializeField] private Sprite[] cracksSprites;

    [Header("Particles")]
    [SerializeField] private ParticleSystem destructionParticle;

    [Header("Others")]
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
            
            //CinemachineShake.instance.ShakeCamera(6f, 0.2f);

            Instantiate(stalactiteTipPf, transform.position, transform.rotation);
            
            destructionParticle.Play();
            canPlayParticleSystem = false;
        }

        //GetComponent<SpriteRenderer>().enabled = false;

        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().sprite = baseStalactiteSprite;

        GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        GetComponent<SpriteRenderer>().sortingOrder = 1;

        GetComponent<PolygonCollider2D>().enabled = false;

        cracksObj.SetActive(false);

        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax);
        //Destroy(this.gameObject);
        //Destroy(transform.parent.gameObject);
    }
}
