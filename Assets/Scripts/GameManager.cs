using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public SceneSwitcher sceneSwitcher;
    [Header("DataBases")]
    public PieceSetDataBase pieceSetData;

    public string selectedMode = "";
    public List <Opening> openings = new ();
    public OpeningTreesData openingTreesData = new();
    public string selcetedOpening = "";

    [Header("Tools")]
    public bool stockFishActivated = false;
    public bool openingDataBaseActivated = false;
    //SingleTon
    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else {
            Destroy(gameObject);
        }

        // Lädt die Umgebungsszene additiv dazu, falls sie noch nicht offen ist
        //if (!SceneManager.GetSceneByName("ChessBoardScene").isLoaded)
        //{
        //    SceneManager.LoadSceneAsync("ChessBoardScene", LoadSceneMode.Additive);
        //}
    
    }
}
