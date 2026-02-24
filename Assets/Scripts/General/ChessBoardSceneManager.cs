using UnityEngine;

public class ChessBoardSceneManager : MonoBehaviour
{
    
    public Board board;
    public static ChessBoardSceneManager instance;

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
