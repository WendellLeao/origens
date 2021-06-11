using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lava : MonoBehaviour
{
    [SerializeField] private TilemapRenderer tileMapLavaCollider;
    [SerializeField] private TilemapRenderer  tileMapLavaNoCollider;
    private string startSortingLayerName;

    void Awake()
    {
        startSortingLayerName = tileMapLavaNoCollider.GetComponent<TilemapRenderer>().sortingLayerName;
    }

    void Update()
    {
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            tileMapLavaNoCollider.GetComponent<TilemapRenderer>().sortingLayerName = startSortingLayerName;
            tileMapLavaCollider.GetComponent<TilemapRenderer>().sortingLayerName = startSortingLayerName;

            tileMapLavaNoCollider.GetComponent<TilemapRenderer>().sortingOrder = 0;
            tileMapLavaCollider.GetComponent<TilemapRenderer>().sortingOrder = 0;
        }
        else
        {
            tileMapLavaNoCollider.GetComponent<TilemapRenderer>().sortingLayerName = "Default";
            tileMapLavaCollider.GetComponent<TilemapRenderer>().sortingLayerName = "Default";

            tileMapLavaNoCollider.GetComponent<TilemapRenderer>().sortingOrder = 2;
            tileMapLavaCollider.GetComponent<TilemapRenderer>().sortingOrder = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
