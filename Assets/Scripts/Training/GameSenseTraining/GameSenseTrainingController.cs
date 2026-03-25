using UnityEngine;
using UnityEngine.UI;
public class GameSenseTrainingController : MonoBehaviour
{

    public bool rotation;
    public bool color;

    public string currentField;
    public Text currentFiledText;
    public RectTransform[] kingImages;
    private int kingIndex;
    public Timer timer;
    public void InitTrainig()
    {
        GetNew();
        timer.timerIsRunning = true;
    }

    public void LakChuch(string currentInput)
    {
        Debug.Log(currentField + " " + currentInput);
        if(currentInput == currentField)
        {
            Debug.Log("Right");
        }
        else
        {
            Debug.Log("Wrong");
        }
        if(timer.timeRemaining == 0)
        {
            EndTrainig();
        }else{

            GetNew();
        }
    }
    public void EndTrainig()
    {
        Debug.Log("EndTraining");
    }
    public void GetNew()
    {
        kingImages[kingIndex].gameObject.SetActive(false);
        rotation = Random.value > 0.5f;
        kingIndex = (Random.value > 0.5f) ? 1:0;
        kingImages[kingIndex].gameObject.SetActive(true);
        SelcetRandomField();
    }
    public void SelcetRandomField()
    {
        int index = Random.Range(0,63);
        currentField = BoardUtil.IndexToString(index);
        currentFiledText.text = currentField;
    }
}
