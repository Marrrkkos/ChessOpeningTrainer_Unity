using System.Collections.Generic;
using UnityEngine;

public class OpeningTrainingCreator : MonoBehaviour
{
    public OpeningTrainingController openingTrainingController;

    public GameObject trainingsPanel;
    public GameObject openingPanel;
    private int depth = 10;
    public void StartOpeningTraining()
    {
        Opening opening = GameManager.instance.selcetedOpening;
        List<List<Move>> allLines = opening.GetAllLines(depth);

        openingTrainingController.InitTraining(allLines, opening);
        SwitchTraining(true);
    }

    public void SwitchTraining(bool trainingActive)
    {
        trainingsPanel.SetActive(trainingActive);
        openingPanel.SetActive(!trainingActive);
    }
}
