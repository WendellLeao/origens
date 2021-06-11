using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform ropePointBB;
    [SerializeField] private Transform ropePointBP_InBB;
    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.sortingLayerName = "BloquinhoBranco";
        lineRenderer.sortingOrder = -2;
    }
    void Start()
    {
        // lineRenderer.sortingLayerName = "BloquinhoBranco";
        // lineRenderer.sortingOrder = -2;
    }
    void Update()
    {
        // lineRenderer.SetPosition(0, ropePointBB.position);
        // lineRenderer.SetPosition(1, BloquinhoPretoController.instance.ropePointBP.position);
        
        if(BloquinhoBrancoController.IsBloquinhoBrancoStatic())
        {
            ////////////////////lineRenderer.SetPosition(1, ropePointBP_InBB.position);

            lineRenderer.sortingLayerName = "BloquinhoBranco";
            lineRenderer.sortingOrder = -2;
        }
        else
        {
            ///////////////////lineRenderer.SetPosition(1, BloquinhoPretoController.instance.ropePointBP.position);
            
            lineRenderer.sortingLayerName = "BloquinhoPreto";
            lineRenderer.sortingOrder = -1;
        }

        if(BloquinhoBrancoController.instance.state == BloquinhoBrancoController.State.Dead)
        {
            lineRenderer.sortingLayerName = "Default";
            lineRenderer.sortingOrder = -5;
        }

    }
    void LateUpdate()
    {
        lineRenderer.SetPosition(0, ropePointBB.position);
        lineRenderer.SetPosition(1, BloquinhoPretoController.instance.ropePointBP.position);
    }
}
