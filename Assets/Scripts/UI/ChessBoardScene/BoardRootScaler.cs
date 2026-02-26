using UnityEngine;
using UnityEngine.UI;

public class BoardRootScaler : MonoBehaviour
{
    public RectTransform canvas;
    public RectTransform content;
    public RectTransform sliderRect;
    public BoardScaler boardScaler;
    public RectTransform board;
    public RectTransform trainingBoardPlacerHolder;
    public RectTransform MainBoardPlacerHolder;
    private float boardEngineSize;
    private float boardTrainingSize;
    private float engineBarWidth;
    void Start()
    {   
        boardTrainingSize = canvas.sizeDelta.x + content.sizeDelta.x;
        engineBarWidth = boardTrainingSize/16f;
        boardEngineSize = boardTrainingSize - engineBarWidth;
        boardScaler.ScaleBoard(boardEngineSize);
        sliderRect.sizeDelta = new Vector2(engineBarWidth, boardEngineSize);

        board.localPosition = new Vector2(-engineBarWidth/2, 0);

        MainBoardPlacerHolder.sizeDelta = new Vector2(boardEngineSize, boardEngineSize);
        MainBoardPlacerHolder.localPosition = new Vector2(-engineBarWidth/2, 0);

        trainingBoardPlacerHolder.sizeDelta = new Vector2(boardTrainingSize, boardTrainingSize);
        trainingBoardPlacerHolder.localPosition  = new Vector2(0, 0);
    }
    public void SetEngineSetUp()
    {
        boardScaler.ScaleBoard(boardEngineSize);
        board.localPosition = new Vector2(engineBarWidth/2, 0);
    }
    public void SetTrainingSetUp()
    {
        boardScaler.ScaleBoard(boardTrainingSize);
        board.localPosition = new Vector2(0, 0);
    }
}
