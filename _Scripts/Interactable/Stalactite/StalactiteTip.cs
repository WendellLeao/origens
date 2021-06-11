using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTip : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem destructionParticle;

    [Header("Others")]
    private bool canPlayParticleSystem = true;

    void Update()
    {
        if(transform.position.y <= -500f)   
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Damage enemy
        IDamageable damageable = other.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.Damage(15);
            OnTriggerEnter_PlayEffects();
        }

        //Damage player
        DamageHandler damageHandler = other.GetComponent<DamageHandler>();

        if(damageHandler != null)
        {
            damageHandler.DamagePlayerPerUnit(80f);
            OnTriggerEnter_PlayEffects();
        }

        //it's the index of the layer named "Ground"
        if(other.gameObject.layer == 8)
        {
            AudioHandler.instance.Play("DestructionFX");
            OnTriggerEnter_PlayEffects();
        }
    }

    private void OnTriggerEnter_PlayEffects()
    {
        //AudioHandler.instance.Play("HitEnemyFX");
        CinemachineShake.instance.ShakeCamera(3f, 0.2f);
        StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        if(canPlayParticleSystem)
        {
            //AudioHandler.instance.Play("DestructionFX");
            
            //CinemachineShake.instance.ShakeCamera(6f, 0.2f);

            destructionParticle.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax);
        Destroy(this.gameObject);
    }
}