using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject crumblingPlatformCracksObj;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem destructionParticle;
    [SerializeField] private float timeToDestroy;

    void OnTriggerEnter2D(Collider2D other) 
    {
        MovementWithKeyboard movement = other.gameObject.GetComponent<MovementWithKeyboard>();

        if(movement != null)
            StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        crumblingPlatformCracksObj.GetComponent<AnimationHandler>().PlayAnimation("DestroyCrumblingPlatformAnim");
        particle.Play();

        yield return new WaitForSeconds(timeToDestroy); 
        
        particle.Stop();
        destructionParticle.Play();
        
        crumblingPlatformCracksObj.SetActive(false);
        
        GetComponent<SpriteRenderer>().enabled = false;        
        
        GetComponent<EdgeCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        AudioHandler.instance.Play("DestructionFX");
        
        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax); 
        Destroy(transform.parent.gameObject);
    }
}

