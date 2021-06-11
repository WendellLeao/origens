using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : MonoBehaviour
{
    WayPointSystem wayPointSystem;

    void Awake()
    {
        wayPointSystem = GetComponent<WayPointSystem>();
    }
    void Update()
    {
        if(wayPointSystem.WayPoints[wayPointSystem.WayPointIndex].position.x > this.transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            if(wayPointSystem.WayPoints[wayPointSystem.WayPointIndex].position.x != this.transform.position.x)
                GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    // private SpriteRenderer elementalSprite;
    // private string startSortingLayerName;

    // void Awake()
    // {
    //     elementalSprite = GetComponent<SpriteRenderer>();
    // }

    // void Start()
    // {
    //     startSortingLayerName = elementalSprite.sortingLayerName;
    // }

    // void Update()
    // {
    //     if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
    //     {
    //         elementalSprite.sortingLayerName = startSortingLayerName;
    //         elementalSprite.sortingOrder = 1;
    //     }
    //     else
    //     {
    //         elementalSprite.sortingLayerName = "Default";
    //         elementalSprite.sortingOrder = 1;
    //     }
    // }
}
