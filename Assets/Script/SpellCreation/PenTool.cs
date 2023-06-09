using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    [Header("Dots")] 
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    
    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform lineParent;
    private LineController currentLine;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponentInParent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            if (currentLine == null)
            {
                currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent)
                    .GetComponent<LineController>();
            }

            GameObject dot = Instantiate(dotPrefab, GetMousePosition(), Quaternion.identity, dotParent);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 worldMousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.z = dotParent.transform.position.z;
        return worldMousePosition;
    }
}
