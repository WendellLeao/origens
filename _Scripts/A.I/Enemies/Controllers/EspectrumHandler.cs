using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspectrumHandler : MonoBehaviour
{
    [Header("Espectrum Handler")]
    [SerializeField] private SpriteRenderer[] objectSprites;
    [SerializeField] private Color espectrumColor;
    [SerializeField] private GameObject espectrumPsObj;
    [SerializeField] private ParticleSystem deathPS;
    private ParticleSystem.MainModule deathPsMainModule;
    private ParticleSystem espectrumPS;
    private SpriteRenderer spriteRenderer;
    private Color startSpriteColor;
    private ParticleSystem.MinMaxGradient startDeathPsColor;
    private Grood grood;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        espectrumPS = espectrumPsObj.GetComponent<ParticleSystem>();

        deathPsMainModule = deathPS.main;

        if(GetComponent<Grood>() != null)
            grood = GetComponent<Grood>();
    }

    void Start()
    {
        startSpriteColor = spriteRenderer.color;
        startDeathPsColor = deathPsMainModule.startColor;

        ChangeSpriteColorToEspectrumColor();
    }

    void Update()
    {
        ChangeSpriteColorHandler();
    }

    private void ChangeSpriteColorHandler()
    {
        EnemyController enemyController = GetComponent<EnemyController>();

        if(!enemyController.isBeingDamaged)
        {
            if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            {
                if (espectrumPsObj != null)
                    espectrumPsObj.SetActive(true);
                
                ChangeSpriteColorToEspectrumColor();
            }
            else
            {
                if (espectrumPsObj != null)
                    espectrumPsObj.SetActive(false);

                ChangeSpriteColorToStartColor();
            }
        }

        //Handle PS
        if (enemyController.GetHealth() > 0f)
            espectrumPS.Play();
        else
            espectrumPS.Stop();
    }

    private void ChangeSpriteColorToEspectrumColor()
    {
        if(objectSprites != null)
        {
            foreach (SpriteRenderer sprite in objectSprites)
            {
                sprite.color = espectrumColor;
            }
        }

        if(grood != null)
            spriteRenderer.color = espectrumColor;

        deathPsMainModule.startColor = espectrumColor;
    }
    private void ChangeSpriteColorToStartColor()
    {
        if(objectSprites != null)
        {
            foreach (SpriteRenderer sprite in objectSprites)
            {
                sprite.color = Color.white;
            }
        }

        if(grood != null)
            spriteRenderer.color = startSpriteColor;

        deathPsMainModule.startColor = startDeathPsColor;
    }
}
