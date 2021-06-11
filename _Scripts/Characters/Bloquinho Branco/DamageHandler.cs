using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DamageHandler : MonoBehaviour
{
    public static DamageHandler instance;
    
    [Header("Health System")]
    [SerializeField] private float maxHealth;
    public HealthSystemPlayer healthSystemPlayer;
    [SerializeField] private Transform healthBarPf;
    [HideInInspector] public Transform healthBarTransform;
    [SerializeField] private float healthBarPosY;
    private HealthBarPlayer healthBarPlayer;

    [Header("Damage and Heal")]
    private bool isBeingDamagedPerSeconds = false;
    private bool isBeingDamagedPerUnit = false;
    private bool playerCanBeDamagedPerUnit = true;
    private bool playerCanBeDamagedPerSecond = true;
    private bool canRegen = false;
    private float timeToRegen;
    private float startTimeToRegen;
    private float damagePerSecond;
    private float damagePerUnit;

    [Header("Particles")]
    [SerializeField] private ParticleSystem destructionParticle;

    //[SerializeField] private GameObject breakableRockCollider;

    [Header("Others")]
    private Image damageImage;
    private float damageImageAlpha = 0f;
    public event Action onPlayerDied;
    private bool willDieInLava = false;
    private bool canPlaySound = true; //false
    private bool canPlayParticleSystem = true;
    private bool deadByDeadZoneGap = false;

    void Awake()
    {
        instance = this;
        
        healthSystemPlayer = new HealthSystemPlayer(maxHealth);

        timeToRegen = 5f; //2f
        startTimeToRegen = timeToRegen;
    }
    void Start()
    {
        if(healthBarPf != null)
        {
            healthBarTransform = Instantiate(healthBarPf, new Vector3(transform.position.x, transform.position.y + healthBarPosY), Quaternion.identity);
            healthBarTransform.transform.parent = this.transform;
                        
            healthBarPlayer = healthBarTransform.GetComponent<HealthBarPlayer>();
            healthBarPlayer.Setup(healthSystemPlayer);
        }

        damageImage = CanvasAssets.instance.damageImage;
    }
    void Update()
    {
        if(healthSystemPlayer.GetHealthFloat() <= 0)
        {
            //onPlayerDied?.Invoke();
            StartCoroutine(Break());
        }

        if(!isBeingDamagedPerUnit)
            playerCanBeDamagedPerUnit = true;

        if(isBeingDamagedPerSeconds)
        {
            if(damagePerSecond <= 0f && damagePerUnit <= 0f)
                isBeingDamagedPerSeconds = false;

            if(canPlaySound && healthSystemPlayer.GetHealthFloat() > 0)
            {
                AudioHandler.instance.Play("PlayerBeHitedFX");
                StartCoroutine(TimeToPlaySoundAgain());
                canPlaySound = false;
            }
        }

        if(willDieInLava)
        {
            //BloquinhoBrancoController.instance.isBloquinhoBranco = true;
            BloquinhoBrancoController.instance.ForceSwitchBloquinhos();
            DamagePlayerPerSeconds(300f);
            //StartCoroutine(Break());
        }

        if(healthSystemPlayer.GetHealthFloat() < healthSystemPlayer.GetHealthMaxFloat())
            HealPlayerHandler();

        DamagePlayerPerSecondsHandler();
        DamageImageHandler();
    }

    public void DamagePlayerPerUnit(float damageAmount)
    {
        if (Dash.instance != null)
        {
            if (!Dash.instance.IsDashing())
            {
                isBeingDamagedPerUnit = true;
            }
        }
        else
        {
            isBeingDamagedPerUnit = true;
        }

        if(isBeingDamagedPerUnit)
        {
            canRegen = false;
            timeToRegen = startTimeToRegen;

            if(playerCanBeDamagedPerUnit)
            {
                healthSystemPlayer.PlayerDamage(damageAmount);
                AudioHandler.instance.Play("PlayerBeHitedFX");
                damageImageAlpha += damageAmount;
            }
            
            isBeingDamagedPerUnit = false;
            playerCanBeDamagedPerUnit = false;
        }
    }
    public void DamagePlayerPerSeconds(float damageAmount)
    {
        if(Dash.instance != null)
        {
            if(!Dash.instance.IsDashing())
                isBeingDamagedPerSeconds = true;
        }
        else
        {
            isBeingDamagedPerSeconds = true;
        }

        if(isBeingDamagedPerSeconds)
            this.damagePerSecond += damageAmount;
    }
    
    public void UnsubscribeDamagePlayerAmount(float damageAmount)
    {
        if(damagePerSecond > 0)
            damagePerSecond -= damageAmount;
        else if(damagePerSecond <= 0f)
            damagePerSecond = 0f;
    }

    private void DamagePlayerPerSecondsHandler()
    {    
        if(isBeingDamagedPerSeconds)
        {
            healthSystemPlayer.PlayerDamagePerSeconds(damagePerSecond);

            damageImageAlpha += Time.deltaTime * damagePerSecond;

            if(healthSystemPlayer.GetHealthFloat() > 0f)
                CinemachineShake.instance.ShakeCamera(4f, 0.2f);

            if (damageImageAlpha >= maxHealth)
                damageImageAlpha = maxHealth;

            canRegen = false;
            timeToRegen = startTimeToRegen;
        }
    }
    
    private void HealPlayerHandler()
    {
        if(!isBeingDamagedPerSeconds && !isBeingDamagedPerUnit)
        {
            RegenTimer();

            if(canRegen)
            {
                healthSystemPlayer.PlayerHeal();
                    
                damageImageAlpha -= Time.deltaTime * 18f;

                if(damageImageAlpha < 0f)
                    damageImageAlpha = 0f;

                if(healthSystemPlayer.GetHealthFloat() > maxHealth)
                    canRegen = false;
            }

            damagePerSecond = 0f;
        }
    }
    private void RegenTimer()
    {
        timeToRegen -= Time.deltaTime; 

        if(timeToRegen <= 0f)
        {
            canRegen = true;
            timeToRegen = 0f;
        }
    }

    private void DamageImageHandler()
    {
        if(BloquinhoBrancoController.instance.state != BloquinhoBrancoController.State.Dead)
            damageImage.color = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, damageImageAlpha * 0.01f);
    }
    public void SetDamageImageAlphaValue(float value)
    {
        damageImage.color = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, value * 0.01f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "DeadZone" || other.gameObject.tag == "DeadZoneGap" || other.gameObject.tag == "Lava")
        {
            canPlaySound = true;
            
            if(canPlaySound)
            {
                AudioHandler.instance.Play("PlayerBeHitedFX");
                canPlaySound = false;
            }
        }

        if(other.gameObject.tag == "DeadZone")
        {
            StartCoroutine(Break());
        }

        if(other.gameObject.tag == "DeadZoneGap")
        {
            onPlayerDied?.Invoke();
            deadByDeadZoneGap = true;
        }

        if(other.gameObject.tag == "Lava")
        {
            BloquinhoPretoController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "BloquinhoBranco";

            if(!GetComponent<BloquinhoBrancoController>().IsGrounded())
            {
                //StartCoroutine(Break());
                //SetDamageImageAlphaValue(100f);
                willDieInLava = true;
                //onPlayerDied?.Invoke();
            }
            else
            {
                if(playerCanBeDamagedPerSecond)
                {
                    DamagePlayerPerSeconds(45f);
                    playerCanBeDamagedPerSecond = false;
                }
            }
        }

        if(other.gameObject.tag == "ImpactWave" || other.gameObject.tag == "DamageableArea")
        {
            BloquinhoBrancoController.instance.bbBody.velocity = new Vector2(
                BloquinhoBrancoController.instance.bbBody.velocity.x, 0f
            );

            BloquinhoBrancoController.instance.bbBody.AddForce(
                new Vector2(BloquinhoBrancoController.instance.bbBody.velocity.x, 170f)
            );
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        
        if(other.gameObject.tag == "Lava")//other.gameObject.tag == "Grood"
        {
            if(isBeingDamagedPerSeconds)
                UnsubscribeDamagePlayerAmount(45f);

            playerCanBeDamagedPerSecond = true;
        }
    }

    private IEnumerator Break()
    {
        onPlayerDied?.Invoke();

        if(healthBarPlayer != null)
            healthBarPlayer.DestroyHealthBar();

        GetComponent<SpriteRenderer>().enabled = false;

        if(BloquinhoPretoController.instance != null)
            BloquinhoPretoController.instance.gameObject.SetActive(false);
        
        if(canPlayParticleSystem)
        {
            AudioHandler.instance.Play("DestructionFX");
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            
            destructionParticle.Play();
            canPlayParticleSystem = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(destructionParticle.main.startLifetime.constantMax);
    }

    IEnumerator TimeToPlaySoundAgain()
    {
        yield return new WaitForSeconds(0.7f);
        canPlaySound = true;
    }

    public bool DeadByDeadZoneGap
    {
        get{return this.deadByDeadZoneGap;}
        set{this.deadByDeadZoneGap = value;}
    }
}
