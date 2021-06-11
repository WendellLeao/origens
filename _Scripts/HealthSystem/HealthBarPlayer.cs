using UnityEngine;

public class HealthBarPlayer : MonoBehaviour
{
    private HealthSystemPlayer healthSystemPlayer;
    [SerializeField] private GameObject bar, background, border;

    public void Setup(HealthSystemPlayer healthSystemPlayer)
    {
        this.healthSystemPlayer = healthSystemPlayer;

        healthSystemPlayer.OnHealthChanged += OnHealthChanged_UpdateHealthBar;
    }

    void Update()
    {
        if(healthSystemPlayer.GetHealthFloat() < healthSystemPlayer.GetHealthMaxFloat())
            ShowHealthBar();
        else
            HideHealthBar();

        if(healthSystemPlayer.GetHealthFloat() <= 0f)
            Destroy(this.gameObject);
    }

    private void ShowHealthBar()
    {
        bar.SetActive(true);
        border.SetActive(true);
        background.SetActive(true);
    }
    private void HideHealthBar()
    {
        bar.SetActive(false);
        border.SetActive(false);
        background.SetActive(false);
    }

    private void OnHealthChanged_UpdateHealthBar(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(healthSystemPlayer.GetHealthPercent(), 1);
    }

    public void DestroyHealthBar()
    {
        Destroy(this.gameObject);
    }
    
    void OnDisable()
    {
        healthSystemPlayer.OnHealthChanged -= OnHealthChanged_UpdateHealthBar;
    }
}
