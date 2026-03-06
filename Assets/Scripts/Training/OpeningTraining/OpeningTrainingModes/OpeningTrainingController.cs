using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TrainingMode
{
    Testing = 0,
    Randomized = 1,
    Learning = 2
}
public class OpeningTrainingController : MonoBehaviour
{
    public RootSelecter rootSelecter;

    
    [Header("Board")]
    public Board board;
    public BoardScaler boardScaler;

    [Header("TrainingPanel")]
    public Text percentNumber;
    public Text time;
    public Text openingName;
    public Text possibleLines;

    [Header("OpeningResult")]
    public OpeningResultController openingResultController;



    public OpeningRandomizedMode openingRandomizedMode = new();
    public OpeningTestingMode openingTestingMode = new();
    public OpeningLearningMode openingLearningMode = new();

    private Node currentNode = new();
    private List<Node> queue = new();
    private int openingLeafSize = 0;
    private int openingNodeSize = 0;
    
    private int lineCounter;
    private float rightCounter;
    private float timer;


    //Constructor
    private Opening opening;
    private int depth;
    private TrainingMode mode;
    public void InitTraining(Opening opening, int depth, TrainingMode mode)
    {

        if(opening.rootNode.children.Count == 0){rootSelecter.SetOpening(); return; }

        switch (mode)
        {
            case TrainingMode.Testing:
                openingTestingMode.InitTraining(opening, depth);
                break;
            case TrainingMode.Randomized:
                openingRandomizedMode.InitTraining(opening, depth);
                break;
            case TrainingMode.Learning:
                openingLearningMode.InitTraining(opening, depth);
                break;
        }
        
        
    }
    public void ManageNext()
    {
        switch (mode)
        {
            case TrainingMode.Testing:
                openingTestingMode.ManageNext();
                break;
            case TrainingMode.Randomized:
                openingRandomizedMode.ManageNext();
                break;
            case TrainingMode.Learning:
                openingLearningMode.ManageNext();
                break;
        }
    }
    
    public void ResetTraining(bool restart)
    {
        switch (mode)
        {
            case TrainingMode.Testing:
                openingTestingMode.ResetTraining(restart);
                break;
            case TrainingMode.Randomized:
                openingRandomizedMode.ResetTraining(restart);
                break;
            case TrainingMode.Learning:
                openingLearningMode.ResetTraining(restart);
                break;
        }
    }
    
}