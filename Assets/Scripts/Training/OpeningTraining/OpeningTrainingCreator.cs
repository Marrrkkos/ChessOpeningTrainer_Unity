using System.Collections.Generic;
using UnityEngine;

public class OpeningTrainingCreator : MonoBehaviour
{
    public OpeningTrainingController openingTrainingController;

    public int depth = 10;
    public void StartOpeningTraining()
    {
        Opening opening = GameManager.instance.selcetedOpening;

        openingTrainingController.InitTraining(opening, depth, TrainingMode.Testing);
    }

}
