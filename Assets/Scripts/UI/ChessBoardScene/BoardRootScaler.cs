using UnityEngine;
using UnityEngine.UI;

public class BoardRootScaler : MonoBehaviour
{
    [Header("General")]
    public RectTransform canvas;
    public RectTransform content;
    
    [Header("Main Board")]
    public BoardScaler boardScaler;
    public RectTransform boardRect;
    public RectTransform engineSliderRect;
    public RectTransform controlButtons;
    public RectTransform rotateButton;

    [Header("Main Scene")]

    public RectTransform top;
    public RectTransform bot;


    private float boardEngineSize;
    private float boardTrainingSize;
    private float engineBarWidth;

    void Start()
    {   
        PickContentSize();



        boardTrainingSize = content.sizeDelta.x;


        engineBarWidth = boardTrainingSize/16f;
        boardEngineSize = boardTrainingSize - engineBarWidth;
        boardScaler.ScaleBoard(boardEngineSize);
        engineSliderRect.sizeDelta = new Vector2(engineBarWidth, boardEngineSize);

        
        top.sizeDelta = new Vector2(content.sizeDelta.x, boardEngineSize/8*5);
        top.localPosition = Vector2.zero;

        boardRect.sizeDelta = new Vector2(boardEngineSize, boardEngineSize);
        boardRect.localPosition = new Vector2(0, -boardEngineSize/8*5);

        engineSliderRect.localPosition = new Vector2(boardRect.sizeDelta.x,-boardEngineSize/8*5);

        controlButtons.sizeDelta = new Vector2(boardEngineSize*5/8, boardEngineSize/8);
        controlButtons.localPosition = new Vector2(boardEngineSize/2,  - (boardEngineSize + top.sizeDelta.y));

        rotateButton.sizeDelta = new Vector2(boardEngineSize/8, boardEngineSize/8);
        rotateButton.localPosition = new Vector2(0, - (boardEngineSize + top.sizeDelta.y));
        
        bot.localPosition = new Vector2(0,  - (boardEngineSize + top.sizeDelta.y + controlButtons.sizeDelta.y));
        bot.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y - boardRect.sizeDelta.y - top.sizeDelta.y - controlButtons.sizeDelta.y);
    }

    private void PickContentSize()
    {
        if(canvas.sizeDelta.y >= canvas.sizeDelta.x * 2)
        {
            content.sizeDelta = new Vector2(canvas.sizeDelta.x - 50, canvas.sizeDelta.y - 200);
            content.localPosition = new Vector2(25, -150);
        }
        else
        {
            float contentHeight = canvas.sizeDelta.y - 200;
            content.sizeDelta = new Vector2(contentHeight / 2, contentHeight);
            content.localPosition = new Vector2((canvas.sizeDelta.x - contentHeight / 2)/2, -150);
        }
        
        
    }
    public void SetEngineSetUp()
    {
        boardScaler.ScaleBoard(boardEngineSize);
        boardRect.localPosition = new Vector2(-engineBarWidth/2, 0);
    }
    public void SetNoEngineSetUp()
    {
        boardScaler.ScaleBoard(boardTrainingSize);
        boardRect.localPosition = new Vector2(0, 0);
    }
}
