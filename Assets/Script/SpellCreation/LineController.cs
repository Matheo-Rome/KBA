using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private List<Slider> sliders;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 6;
    }

    private void Update()
    {
        for (int i = 0; i < sliders.Count; i++) 
        {
            lr.SetPosition(i, sliders[i].handleRect.transform.position);
        }
        
    }
}
    
    
