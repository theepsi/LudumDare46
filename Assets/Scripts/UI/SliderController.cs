using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Image slider;
    public TextMeshProUGUI label;

    private float maxValue;
    private float minValue;

    private float currentValue;

    public void Init(string labelText, float minValue, float maxValue, float nonNormalizedValue)
    {
        label.text = labelText;
        this.minValue = minValue;
        this.maxValue = maxValue;

        SetValue(nonNormalizedValue);
    }

    public float GetValue()
    {
        return currentValue;
    }

    public void SetValue(float value)
    {
        float normalizedValue = (value - minValue) / maxValue;
        currentValue = slider.fillAmount = normalizedValue;
    }
}
