using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetSliderMax(float max)
    {
        if (slider)
        {
            slider.maxValue = max;
            slider.value = max;
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
