using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class SliderControllerUI : MonoBehaviour
{
    public Slider slider;
    public InputField sliderValueInputField;
    public Text maxText;

    private long max = 0;
    private long currentValue = 0;

    public void Init(long min, long max, long currentValue)
    {
        this.max = max;
        slider.maxValue = max;
        slider.minValue = min;
        this.currentValue = currentValue;
    }
    public void OnValueChangedSlider()
    {
        currentValue = (long)slider.value;
        SetText();
    }
    public void OnValueChangedInputField()
    {
        currentValue = long.Parse(sliderValueInputField.text);
        slider.value = currentValue;
        SetText();
    }
    public void SetText()
    {

        if(currentValue == max)
        {
            maxText.gameObject.SetActive(true);
            sliderValueInputField.textComponent.gameObject.SetActive(false);
        }
        else
        {
            maxText.gameObject.SetActive(false);
            sliderValueInputField.textComponent.gameObject.SetActive(true);
            sliderValueInputField.text = currentValue.ToString();
        }
    }
    public void setFillStand(long fillStand)
    {
        if (max <= 0) return;

        float progress = (float)((double)fillStand / max);
        slider.value = progress;
    }
}