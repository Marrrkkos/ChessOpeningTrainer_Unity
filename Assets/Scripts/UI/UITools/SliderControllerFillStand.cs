using System;
using UnityEngine;

using UnityEngine.UI;

public class SliderControllerFillStand : MonoBehaviour
{
    public Slider slider;
    private long max = 0;
    private long min = 0;
    public void SetMinMax(long min, long max)
    {
        this.max = max;
        this.min = min;
        slider.minValue = 0;
        slider.maxValue = 1;
    }

    public void SetFillStand(long fillStand)
    {
        if (max <= 0) return;

        float progress = (float)((double)fillStand+max) / (Math.Abs(max)+Math.Abs(min));

        Debug.Log("fillstand:" + fillStand + " Progress:" + progress);
        slider.value = progress;
    }
}