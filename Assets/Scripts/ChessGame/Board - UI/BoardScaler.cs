using UnityEngine;

public class BoardScaler : MonoBehaviour
{
    public RectTransform board;

    public float cellSize;
    private int currentRotation = 0;
    public void scaleBoard(float boardSize) { 
        board.sizeDelta = new Vector2(boardSize, boardSize);
        cellSize = boardSize/8;
    }
    public void rotate()
    {

        if (currentRotation == 0)
        {
            board.localEulerAngles = new Vector3(0, 0, 180);
            GameManager.instance.mainBoard.rotation = true;
            currentRotation = 180;
        }
        else if (currentRotation == 180)
        {
            board.localEulerAngles = new Vector3(0, 0, 0);
            GameManager.instance.mainBoard.rotation = false;
            currentRotation = 0;
        }
        rotatePieces(currentRotation);
    }
    private void rotatePieces(int rotation)
    {
        Field[] fields = GameManager.instance.mainBoard.fields;
        for (int i = 0; i < 64; i++)
        {
            fields[i].pieceImage.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotation);
        }
    }



}
