using System.Collections;
using UnityEngine;

public class DashTrigger : MonoBehaviour
{
    [Header("Bloquinhos")]
    [SerializeField] private Transform bbPos;
    private bool canShakeCamera = false;

    void Update()
    {
        if(canShakeCamera)
        {
            //CinemachineShake.instance.ShakeCamera(6f, 0.2f);
            StartCoroutine("StopShakeCamera");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            bbPos.parent = null;
            BloquinhoBrancoController.instance.bpPos.parent = null;
            
            damageable.Damage(100);
            
            canShakeCamera = true;
        }
    }

    IEnumerator StopShakeCamera()
    {
        yield return new WaitForSeconds(0.2f);
        canShakeCamera = false;
    }
}
