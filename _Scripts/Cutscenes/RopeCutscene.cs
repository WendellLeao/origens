using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCutscene : MonoBehaviour
{
    [Header("Components")]
    LineRenderer lineRenderer;
    [SerializeField] private Transform ropePointBB;
    [SerializeField] private Transform ropePointBP;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        lineRenderer.SetPosition(0, ropePointBB.position);
        lineRenderer.SetPosition(1, ropePointBP.position);
    }
}
