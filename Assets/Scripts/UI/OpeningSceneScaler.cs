using UnityEngine;

public class OpeningSceneScaler : MonoBehaviour
{
    public BoardScaler boardScaler;

    public void Awake()
    {
        boardScaler.ScaleBoard(1080f);
    }
}