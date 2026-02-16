using UnityEngine;
using UnityEngine.UI;

public class BoardScaler : MonoBehaviour
{
    public RectTransform boardRect;
    public Board board;
    public GridLayoutGroup gridLayoutGroup;
    public float cellSize;
    private int currentRotation = 0;
    public void ScaleBoard(float boardSize) { 
        
        boardRect.sizeDelta = new Vector2(boardSize, boardSize);
        cellSize = boardSize/8;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
    public void rotate()
    {

        if (currentRotation == 0)
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 180);
            board.rotation = true;
            currentRotation = 180;
        }
        else if (currentRotation == 180)
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 0);
            board.rotation = false;
            currentRotation = 0;
        }
        rotatePieces(currentRotation);
    }
    private void rotatePieces(int rotation)
    {
        Field[] fields = board.fields;
        for (int i = 0; i < 64; i++)
        {
            fields[i].pieceImage.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotation);
        }
    }



}
