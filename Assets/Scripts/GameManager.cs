using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [Header("Board")]
    public Board mainBoard;
    public List<Game> gameDataBase = new List<Game>();
    public int gameIndex = 0;

    [Header("DataBases")]
    public PieceSetDataBase pieceSetData;

    public List <Opening> openings = new ();
    public OpeningsTreesData openingTreesData = new();
    public OpeningsManager openingsManager;

    public SnapPreview snapPreview;
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
