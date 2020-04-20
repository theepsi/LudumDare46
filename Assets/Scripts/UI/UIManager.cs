using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public SliderController hullSlider;
    public SliderController oxygenSlider;

    public void Init(float max_hull, float min_hull, float max_oxygen, float min_oxygen)
    {
        hullSlider.Init("Hull", min_hull, max_hull, max_hull);
        oxygenSlider.Init("O2", min_oxygen, max_oxygen, max_oxygen);

        EventManager.StartListening(Statics.Events.hullDamaged, UpdateHullSlider);
        EventManager.StartListening(Statics.Events.oxygenLost, UpdateOxygenSlider);
    }

    private void UpdateHullSlider(object value)
    {
        hullSlider.SetValue((float)value);
    }

    private void UpdateOxygenSlider(object value)
    {
        oxygenSlider.SetValue((float)value);
    }
}
