using UnityEngine;

public class PlayerParentDetector : MonoBehaviour
{
    void Update()
    {
        if(!BloquinhoBrancoController.instance.IsGrounded())// && !BloquinhoBrancoController.instance.IsOnMovingPlatform()
        {
            if(Input.GetAxisRaw("Horizontal") > 0.25f || Input.GetAxisRaw("Horizontal") < -0.25f)
                BloquinhoBrancoController.instance.transform.parent = null;
        }    
    }

    #region Collision (Commented)
    void OnCollisionEnter2D(Collision2D other)
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
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")   
        {
            other.transform.parent = null;

            if(BloquinhoPretoController.instance != null)
                BloquinhoPretoController.instance.transform.parent = null;
        } 
    }
    #endregion
}
