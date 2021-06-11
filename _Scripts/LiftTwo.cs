using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTwo : MonoBehaviour
{
    void Update()
    {
        Vector3 newPos = new Vector3(0f, 3f, 0f);
        transform.position += (newPos * Time.deltaTime);        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(BloquinhoBrancoController.instance.IsOnMovingPlatform())
            {
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
