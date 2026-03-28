using Unity.VisualScripting;
using UnityEngine;

public class RootSelecter : MonoBehaviour
{
    [Header("General GameObjects")]
    public GameObject InGameSettings;

    public GameObject MainPanel;

    [Header("Opening GameObjects")]
    public GameObject OpeningTrainingCreator;
    public GameObject OpeningTraining;
    public GameObject OpeningResult;
    [Header("GameSense GameObjects")]
    public GameObject GameSense;
    [Header("GameView GameObjects")]
    //public GameObject GameView;

    [Header("Loaders")]
    public OpeningLoader openingLoader;

    [Header("MiniGamesInitializer")]
    public GameSenseTrainingController gameSenseTrainingController;
    public BoardRootScaler boardRootScaler;

    public void OnEnable()
    {
        string selected = GameManager.instance.selectedMode;
        if(selected == "GameSense")
        {
            GameSense.SetActive(true);
            gameSenseTrainingController.InitTrainig();
        }else if( selected == "Opening")
        {
            MainPanel.SetActive(true);
            openingLoader.LoadOpening();
        }else if(selected == "GameView")
        {
            //GameView.SetActive(true);
        }
    }
    public void OnDisable()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        
        //boardRootScaler.SetEngineSetUp();

        MainPanel.SetActive(false); 
        //GameSense.SetActive(false);
        //GameView.SetActive(false);
        InGameSettings.SetActive(false);
        OpeningResult.SetActive(false);
        OpeningTraining.SetActive(false);
        OpeningTrainingCreator.SetActive(false);
    }
    public void SetOpeningTraining()
    {
        OpeningResult.SetActive(false);
        OpeningTrainingCreator.SetActive(false);
        MainPanel.SetActive(false); 
        OpeningTraining.SetActive(true);

        //boardRootScaler.SetTrainingSetUp();
    }
    public void SetGameSenseTraining()
    {
        
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
        MainPanel.SetActive(true);
        
         //boardRootScaler.SetEngineSetUp();
    }
    public void SetIngameSettings()
    {
        InGameSettings.SetActive(true);
    }
    
}