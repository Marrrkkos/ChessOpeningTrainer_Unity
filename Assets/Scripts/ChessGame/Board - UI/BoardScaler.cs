using UnityEngine;

public class BoardScaler : MonoBehaviour
{
    public RectTransform board;

    public float cellSize;

    public void scaleBoard(float boardSize) { 
        board.sizeDelta = new Vector2(boardSize, boardSize);
        cellSize = boardSize/8;
    }




}
