using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private Swing sw;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = sw.maxfuel;
        slider.minValue = 0;
        slider.value = sw.fuel;
    }

    public void IncrementFuel(float newFuel)
    {
        slider.value = newFuel;
    }

    public void DecrementFuel(float newFuel)
    {
         slider.value = newFuel;
    }
}
