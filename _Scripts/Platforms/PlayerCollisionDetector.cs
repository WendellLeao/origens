using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [SerializeField] private WayPointSystem wayPointSystem;

    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.gameObject.tag == "Player")
    //     {
    //         if(BloquinhoBrancoController.instance.IsOnMovingPlatform())
    //         {
    //             if (!wayPointSystem.CanStart || wayPointSystem.CanStartTrigged)
    //                 wayPointSystem.CanStartTimer = true;
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(BloquinhoBrancoController.instance.IsOnMovingPlatform())
            {
                if (!wayPointSystem.CanStart || wayPointSystem.CanStartTrigged)
                    wayPointSystem.CanStartTimer = true;

                other.transform.parent = this.transform;

                if(BloquinhoPretoController.instance != null)
                    BloquinhoPretoController.instance.transform.parent = this.transform;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")   
        {
            other.transform.parent = null;

            if(BloquinhoPretoController.instance != null)
                BloquinhoPretoController.instance.transform.parent = null;
        } 
    }
}
