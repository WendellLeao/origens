using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    [SerializeField] private float speedToSink;
    [SerializeField] private float timeToSink;
    private float startTimeToSink;
    private bool canStartCounting = false;
    private bool canSink = false;
    private Vector2 startPos;

    void Awake()
    {
        startPos = transform.position;
        startTimeToSink = timeToSink;
    }
    
    void Update()
    {
        if(canStartCounting && transform.position.y >= startPos.y)
        {
            SinkTimer();
        }        
    }

    void FixedUpdate()
    {
        if(canSink)
        {
            transform.Translate(new Vector2(0f, -speedToSink * Time.deltaTime));
        }
        else
        {
            if(transform.position.y < startPos.y)
                transform.Translate(new Vector2(0f, speedToSink * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        MovementWithKeyboard movement = other.gameObject.GetComponent<MovementWithKeyboard>();

        if(movement != null) //&& movement.IsOnPlatform()
        {
            if(transform.position.y >= startPos.y)
                canStartCounting = true;
            else
                canSink = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        MovementWithKeyboard movement = other.gameObject.GetComponent<MovementWithKeyboard>();

        if(movement != null)
        {
            timeToSink = startTimeToSink;
            canStartCounting = false;
            canSink = false;
        }
    }

    private void SinkTimer()
    {
        timeToSink -= Time.deltaTime; 

        if(timeToSink <= 0f)
        {
            canSink = true;
            timeToSink = 0f;
        }
        else
        {
            ShakePlatform();
        }
    }

    private void ShakePlatform()
    {
        transform.position = new Vector2(Mathf.Lerp(startPos.x - 0.05f, startPos.x + 0.05f, Mathf.PingPong(Time.time * 12f , 1)), transform.position.y);
    }
}
