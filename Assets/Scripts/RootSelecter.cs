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
        Opening.SetActive(false); 
        GameSense.SetActive(false);
        GameView.SetActive(false);
    }

    public void SetDefault()
    {
        
    }
    public void SetOpeningTraining()
    {
        
    }
    public void SetOpeningTrainingCreation()
    {
        
    }
    public void SetOpeningResult()
    {
        
    }
}