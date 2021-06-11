using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jump))]
public class ImproveJumpPhysics : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D body;

    [Header("Values")]
    [SerializeField] private float fallMultiplier; 
    [SerializeField] private float lowJumpMultiplier; 

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (body.velocity.y < 0)
            body.gravityScale = fallMultiplier;

        else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
            body.gravityScale = lowJumpMultiplier;

        else
            body.gravityScale = 1f;
    }
}
