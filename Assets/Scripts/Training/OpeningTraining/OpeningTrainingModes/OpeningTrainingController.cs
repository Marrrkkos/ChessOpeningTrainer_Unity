using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
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

    public OpeningRandomizedMode openingRandomizedMode;
    public OpeningTestingMode openingTestingMode;
    public OpeningLearningMode openingLearningMode;

    private IOpeningTrainer currentOpeningTrainer;
    //Constructor
    private Opening opening;
    private TrainingMode mode;




    private IPlayer whitePlayer;
    private IPlayer blackPlayer;
    private IPlayer currentPlayer;

    private bool isGameRunning = false;
    private Move pendingMove = null;
    public void InitTraining(Opening opening, int depth, TrainingMode mode)
    {

        if(opening.rootNode.children.Count == 0){rootSelecter.SetOpening(); return; }

        this.mode = mode;
        board.gameController.openingTrainingActive = true;

        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        
        switch (mode)
        {
            case TrainingMode.Learning:
                currentOpeningTrainer = openingLearningMode;
                break;
            case TrainingMode.Testing:
                currentOpeningTrainer = openingTestingMode;
                break;
            case TrainingMode.Randomized:
                currentOpeningTrainer = openingRandomizedMode;
                break;
        }

        currentOpeningTrainer.InitTraining(opening, depth, board, boardScaler);
        
        StartCoroutine(GameLoop());
        
    }
    private IEnumerator GameLoop()
    {
        while (isGameRunning)
        {
            pendingMove = null;

            currentOpeningTrainer.ManageNext();

            while (pendingMove == null)
            {
                yield return null; 
            }

            board.DoMove(pendingMove, true, true);


            // Prüf ob game zu ende ist

            currentPlayer = (currentPlayer == whitePlayer) ? blackPlayer : whitePlayer;
        }
    }
    public void ManageNext()
    {
        currentOpeningTrainer.ManageNext();
    }
    
    public void ResetTraining(bool restart)
    {
        board.gameController.openingTrainingActive = restart;

        
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);


        currentOpeningTrainer.ResetTraining();
    }
    
    
}