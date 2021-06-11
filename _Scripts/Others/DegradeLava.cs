using UnityEngine;

public class DegradeLava : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        if(BloquinhoBrancoController.instance != null)
            BloquinhoBrancoController.instance.onBloquinhosChanged += OnBloquinhosChanged_DisableSpriteRenderer;
    }

    private void OnBloquinhosChanged_DisableSpriteRenderer()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
            spriteRenderer.enabled = true;
        else
            spriteRenderer.enabled = false;
    }

    void OnDisable()
    {
        if(BloquinhoBrancoController.instance != null)
            BloquinhoBrancoController.instance.onBloquinhosChanged -= OnBloquinhosChanged_DisableSpriteRenderer;
    }
}
