using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Health System")]
    [SerializeField] private int health;
    private GameObject commonEnergyPf; //[SerializeField] 
    protected HealthSystem healthSystem;
    [SerializeField] private Transform healthBarPf;
    private Transform healthBarTransform;
    protected HealthBar healthBar;
    [SerializeField] protected float healthBarPosY;
    private Color startSpriteColor;
    private bool canSpawnCommonEnergy = false;
    [HideInInspector] public bool isBeingDamaged {get; private set;} = false;

    protected virtual void Awake()
    {
        healthSystem = new HealthSystem(health);

        startSpriteColor = GetComponent<SpriteRenderer>().color;
    }
    protected virtual void Start()
    {
        if(healthBarPf != null)
        {
            healthBarTransform = Instantiate(healthBarPf, new Vector3(transform.position.x, transform.position.y + healthBarPosY), Quaternion.identity);            
            healthBarTransform.transform.parent = this.transform;
                        
            //HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
            healthBar = healthBarTransform.GetComponent<HealthBar>();
            healthBar.Setup(healthSystem);
        }
    }
    protected virtual void Update()
    {
        CheckHealth();
    }

    #region InterfaceMethods
    public void Damage()
    {
        this.EnemyDamage();
    }
    public void Damage(int damageAmount)
    {
        this.EnemyDamage(damageAmount);
    }
    #endregion

    private void CheckHealth()
    {
        if(healthSystem.GetHealth() <= 0)
        {
            //if(commonEnergyPf != null)
                //SpawnCommonEnergy();

            if(BloquinhoBrancoController.instance.GetComponent<Transform>().parent == this.transform)
            {
                BloquinhoBrancoController.instance.GetComponent<Transform>().parent = null;
                BloquinhoBrancoController.instance.IsBeingGrabbed = false;
            }
            
            CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            Destroy(gameObject);
        }
    }

    protected virtual void EnemyDamage()
    {
        healthSystem.Damage();
        StartCoroutine(EnemyDamageAnim());
    }
    public void EnemyDamage(int enemyDamageAmount)
    {
        healthSystem.Damage(enemyDamageAmount);
    }

    protected float PlayerDistance(Transform playerPos)
    {
        return Vector2.Distance(playerPos.position, transform.position);
    }
    protected float PlayerDistanceY(Transform playerPos)
    {
        return transform.position.y - playerPos.position.y;
    }

    private void SpawnCommonEnergy()
    {
        canSpawnCommonEnergy = true;

        if(canSpawnCommonEnergy)
        {
            GameObject cloneCommomEnergy = Instantiate(commonEnergyPf, transform.position, transform.rotation);
            cloneCommomEnergy.GetComponent<CommonEnergy>().CanGoToBB = true;
            canSpawnCommonEnergy = false;
        }
    }

    protected virtual void Flip()
    {
        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        
        transform.localScale = theScale;

        FlipHealthBar();
    }

    protected void FlipHealthBar()
    {
        Vector3 healthBarTransformScale = healthBarTransform.localScale;

        healthBarTransformScale.x *= -1;
        
        healthBarTransform.localScale = healthBarTransformScale;
    }
    public void EnemyFlip()
    {
        Flip();
    }
    protected virtual IEnumerator EnemyDamageAnim()
    {
        isBeingDamaged = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = startSpriteColor;
        isBeingDamaged = false;
    }

    public float GetHealth()
    {
        return healthSystem.GetHealth();
    }
}
