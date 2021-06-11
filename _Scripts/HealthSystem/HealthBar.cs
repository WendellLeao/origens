using UnityEngine;

public class HealthBar: MonoBehaviour
{
    private HealthSystem healthSystem;
    [SerializeField] private GameObject bar, background, border;
    private bool canSubscribeEvent = false;

    public void Setup(HealthSystem HealthSystem)
    {
        this.healthSystem = HealthSystem;

        canSubscribeEvent = true;

        HealthSystem.OnHealthChanged += OnHealthChanged_UpdateHealthBar;
    }

    void OnEnable()
    {
        if(canSubscribeEvent)
            healthSystem.OnHealthChanged += OnHealthChanged_UpdateHealthBar;
    }

    void Update()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            bar.SetActive(false);
            border.SetActive(false);
            background.SetActive(false);
        }
        else if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic() && healthSystem.GetHealth() < healthSystem.GetHealthMax())
        {
            bar.SetActive(true);
            border.SetActive(true);
            background.SetActive(true);
        }
    }

    private void OnHealthChanged_UpdateHealthBar(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }

    public void DestroyHealthBar()
    {
        Destroy(this.gameObject);
    }
    void OnDisable()
    {
        healthSystem.OnHealthChanged -= OnHealthChanged_UpdateHealthBar;
    }
}
