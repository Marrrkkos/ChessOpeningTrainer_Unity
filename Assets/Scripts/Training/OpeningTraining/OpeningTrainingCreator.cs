using System.Collections.Generic;
using UnityEngine;

public class OpeningTrainingCreator : MonoBehaviour
{
    public OpeningTrainingController1 openingTrainingController1;

    private int depth = 10;
    public void StartOpeningTraining()
    {
        Opening opening = GameManager.instance.selcetedOpening;
        List<List<Move>> allLines = opening.GetAllLines(depth);

        openingTrainingController1.InitTraining(opening);
    }

}
