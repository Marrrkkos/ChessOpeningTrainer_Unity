using UnityEngine;
using UnityEngine.UI;
public class OpeningResultController : MonoBehaviour
{
    public OpeningTrainingController openingTrainingController;

    public Text AllLines;
    public Text RightLines;
    public Text WrongLines;
    public Text Percentage;
    public Text time;
    

    public void SetResult(float allLines, float rightLines, float time)
    {
        AllLines.text = allLines + "";
        RightLines.text = rightLines + "";
        WrongLines.text = allLines-rightLines + "";
        Percentage.text = (rightLines/allLines) * 100 + "%";
        this.time.text = time + "";
    }

    public void RestartTrainig()
    {
        openingTrainingController.ResetTraining(true);
    }
    public void Continue()
    {
        openingTrainingController.ResetTraining(false);
    }
}