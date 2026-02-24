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
    public Opening selcetedOpening = new();

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
    
    }
}
