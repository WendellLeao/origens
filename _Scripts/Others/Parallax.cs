using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private bool ignoreY;
    [SerializeField] private float parallaxEffect;
    private Vector3 startPos;
    private float lenght; 
    Transform camPos;

    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        camPos = Camera.main.transform;
    }

    void Update()
    {
        float temp = (camPos.transform.position.x * (1 - parallaxEffect));
        float distance = (camPos.transform.position.x * parallaxEffect);

        if(ignoreY)
            transform.position = new Vector3(startPos.x + distance, startPos.y, transform.position.z);
        else
            transform.position = new Vector3(startPos.x + distance, transform.position.y, transform.position.z);

        if(temp > startPos.x + lenght) startPos.x += lenght;
        else if(temp < startPos.x - lenght) startPos.x -= lenght;
    }
}
