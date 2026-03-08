using UnityEngine;

using UnityEngine.UI;

public class SliderControllerFillStand : MonoBehaviour
{
    public Slider slider;
    private long max = 0;

    public void setMax(long max)
    {
        this.max = max;

        slider.minValue = 0;
        slider.maxValue = 1;
    }

    public void setFillStand(long fillStand)
    {
        if (max <= 0) return;

        float progress = (float)((double)fillStand / max);
        slider.value = progress;
    }
}