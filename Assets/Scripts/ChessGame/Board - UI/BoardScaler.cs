using UnityEngine;
using UnityEngine.UI;

public class BoardScaler : MonoBehaviour
{
    public RectTransform boardRect;
    public Board board;
    public GridLayoutGroup gridLayoutGroup;
    public float cellSize;
    public void ScaleBoard(float boardSize) { 
        
        boardRect.sizeDelta = new Vector2(boardSize, boardSize);
        cellSize = boardSize/8;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
    public void SetRotation(bool rotation)
    {
        if(rotation)
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 180);
            board.rotation = true;
            RotatePieces(180);
        }
        else
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 0);
            board.rotation = false;
            RotatePieces(0);
        }
    }
    private void RotatePieces(int rotation)
    {
        Field[] fields = board.fields;
        for (int i = 0; i < 64; i++)
        {
            fields[i].pieceImage.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotation);
        }
    }



}
