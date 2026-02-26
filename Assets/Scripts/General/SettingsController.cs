using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider musicStrength;
    public Slider soundStrength;


    public Slider engineArrowCountSlider;
    public Slider engineStrengthSlider;



    public void SaveSettings()
    {
        //GENERAL
        GameManager.instance.settings.musicStrength = (int)musicStrength.value;
        GameManager.instance.settings.soundStrength = (int)soundStrength.value;

        //STOCKFISH
        GameManager.instance.settings.engineArrowCount = (int)engineArrowCountSlider.value;
        GameManager.instance.settings.engineStrength = (int)engineStrengthSlider.value;
    }
    public void OnEnable()
    {
        //GENERAL
        musicStrength.value = GameManager.instance.settings.musicStrength;
        soundStrength.value = GameManager.instance.settings.soundStrength;

        //STOCKFISH
        engineStrengthSlider.value = GameManager.instance.settings.engineStrength;
        engineArrowCountSlider.value = GameManager.instance.settings.engineArrowCount;
    }
}
