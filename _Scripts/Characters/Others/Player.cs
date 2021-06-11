using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private NormalizeSlope normalizeSlope;
    private Rigidbody2D rb;
  
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalizeSlope = GetComponent<NormalizeSlope>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move(){
        float direction = Input.GetAxis("Horizontal");
        this.normalizeSlope.MoveAndNormalizeSlope(direction,speed);//Left or Right
    }

    void Jump(){
        if(Input.GetKey(KeyCode.Space) && normalizeSlope.grounded){
            rb.AddForce(Vector2.up*600);
        }
    }
}

