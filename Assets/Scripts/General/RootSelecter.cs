using Unity.VisualScripting;
using UnityEngine;

public class RootSelecter : MonoBehaviour
{
    [Header("General GameObjects")]
    public GameObject InGameSettings;

    [Header("Opening GameObjects")]
    public GameObject Opening;
    public GameObject OpeningTrainingCreator;
    public GameObject OpeningTraining;
    public GameObject OpeningResult;
    [Header("GameSense GameObjects")]
    public GameObject GameSense;
    [Header("GameView GameObjects")]
    public GameObject GameView;

    [Header("Loaders")]

    public OpeningLoader openingLoader;

    public void OnEnable()
    {
        string selected = GameManager.instance.selectedMode;
        Debug.Log(selected);
        if(selected == "GameSense")
        {
            GameSense.SetActive(true);
        }else if( selected == "Opening")
        {
            Opening.SetActive(true);
            openingLoader.LoadOpening();
        }else if(selected == "GameView")
        {
            GameView.SetActive(true);
        }
    }
    public void OnDisable()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        Opening.SetActive(false); 
        GameSense.SetActive(false);
        GameView.SetActive(false);
        InGameSettings.SetActive(false);
        OpeningResult.SetActive(false);
        OpeningTraining.SetActive(false);
        OpeningTrainingCreator.SetActive(false);

    }
    public void SetOpeningTraining()
    {
        OpeningResult.SetActive(false);
        OpeningTrainingCreator.SetActive(false);
        Opening.SetActive(false); 
        OpeningTraining.SetActive(true);
    }
    public void SetOpeningTrainingCreation()
    {
        OpeningTrainingCreator.SetActive(true);
    }
    public void SetOpeningResult()
    {
        OpeningResult.SetActive(true);
    }
    public void SetOpening()
    {
        OpeningTrainingCreator.SetActive(false);
        OpeningTraining.SetActive(false);
        OpeningResult.SetActive(false);
        InGameSettings.SetActive(false);
        Opening.SetActive(true); 
    }
    public void SetIngameSettings()
    {
        InGameSettings.SetActive(true);
    }
}