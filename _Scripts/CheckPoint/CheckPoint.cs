using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static event EventHandler onPlayerTriggerEnter;
    public static event EventHandler onPlayerExitTriggerEnter;
    [SerializeField] private float distanceToTriggerX, distanceToTriggerY; 
    private bool canPlayCheckPointAnimation = true; //false
    private Transform bbPos;
    void Start()
    {
        if(BloquinhoBrancoController.instance != null)
            bbPos = BloquinhoBrancoController.instance.GetComponent<Transform>();
    }
    void Update()
    {
        float playerDistance = Mathf.Abs(PlayerDistance(bbPos));
        float playerDistanceY = Mathf.Abs(PlayerDistanceY(bbPos));

        if (playerDistance <= distanceToTriggerX)
        {
            if (playerDistanceY <= distanceToTriggerY)
            {
                CheckPointHandler.instance.LastCheckPointPos = this.transform.position;
                
                /*if(PrimordialEnergyUI.instance != null)
                PrimordialEnergyUI.instance.CanShowPrimordialUI();
            
                if(StaminaSystem.instance != null)
                    StaminaBar.ShowStaminaUIAnimStatic();
    
                if(canPlayCheckPointAnimation)
                {
                    CheckPointUI.ShowCheckPointUIAnimStatic();
                    canPlayCheckPointAnimation = false;
                }*/
    
                onPlayerTriggerEnter?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                //canPlayCheckPointAnimation = true;
                onPlayerExitTriggerEnter?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            //canPlayCheckPointAnimation = true;
            onPlayerExitTriggerEnter?.Invoke(this, EventArgs.Empty);
        }
    }
    private float PlayerDistance(Transform playerPos)
    {
        return Vector2.Distance(playerPos.position, transform.position);
    }
    private float PlayerDistanceY(Transform playerPos)
    {
        return transform.position.y - playerPos.position.y;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        
        Vector3 newPos = new Vector3(distanceToTriggerX, distanceToTriggerY, transform.position.z);
        Gizmos.DrawWireCube(transform.position, newPos);
    }
}
