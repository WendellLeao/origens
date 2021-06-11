using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private Transform player;
    private bool isGrounded;
    private bool check;

    void Update()
    {
        isGrounded = BloquinhoBrancoController.instance.IsGrounded();

        if(!isGrounded)
        {
            check = false;

            if(Input.GetAxisRaw("Horizontal") > 0.25f || Input.GetAxisRaw("Horizontal") < -0.25f)
                player.SetParent(null);
        }

        if(!check)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.125f);

            if(hit.collider != null)
            {
                if(hit.collider.CompareTag("MovingPlatform"))
                {
                    player.SetParent(hit.transform);
                }
                else
                {
                    player.SetParent(null);
                }

                check = true;
            }
        }
    }
}
