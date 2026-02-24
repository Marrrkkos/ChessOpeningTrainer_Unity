using UnityEngine;

using UnityEngine.UI; // Wichtig für die Slider-Komponente

public class SliderController : MonoBehaviour
{
    public Slider slider;
    private long max = 0;

    public void setMax(long max)
    {
        this.max = max;
        // Optional: Wir setzen den Slider-Bereich intern immer auf 0 bis 1
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