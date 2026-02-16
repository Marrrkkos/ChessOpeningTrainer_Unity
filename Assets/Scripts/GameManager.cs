using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [Header("DataBases")]
    public PieceSetDataBase pieceSetData;

    public List <Opening> openings = new ();
    public OpeningTreesData openingTreesData = new();
    public string selcetedOpening = "";
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
