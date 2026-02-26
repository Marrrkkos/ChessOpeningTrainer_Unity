using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    private List<List<Move>> allLines = new();
    private int lineIndex;
    private Opening opening;
    private List<Move> currentLine = new();
    private float rightCounter;
    private float timer;
    public void InitTraining(List<List<Move>> allLines, Opening opening)
    {
        if(allLines.Count == 0){rootSelecter.SetOpening(); return; }

        board.openingTrainingActive = true;
        lineIndex = 0;
        rightCounter = 0;
        timer = 0;
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        this.opening = opening;
        this.allLines = allLines;

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
        }

        currentLine = allLines[lineIndex];


        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + allLines.Count;

        if (!opening.color)
        {
            ManageNext();
        }


    }
    public void ManageNext()
    {
        

        if (!board.currentGame.playedMoves.Last().Equals(currentLine[board.currentGame.playedMoves.Count - 1]))
        {
            GoNextLine(false);
            return;
        }

        
        if(currentLine.Count <= board.currentGame.playedMoves.Count)
        {
            GoNextLine(true);
            return;
        }

        board.doMove(currentLine[board.currentGame.playedMoves.Count], true, true);

        if(currentLine.Count <= board.currentGame.playedMoves.Count)
        {
           GoNextLine(true);
           return;
        }
    }
    private void GoNextLine(bool everythingRight)
    {
        lineIndex++;
        if(everythingRight)
        {
            rightCounter++;
        }

        if(lineIndex >= allLines.Count)
        {
            EndTraining();
            return;
        }
        currentLine = allLines[lineIndex];
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.doMove(m,true,true);
        }

        

        float percent = (rightCounter / lineIndex) * 100;
        percentNumber.text = percent + "%";
        possibleLines.text = lineIndex + "/" + allLines.Count;

        if (!opening.color)
        {
            ManageNext();
        }
    }

    private void EndTraining()
    {
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.doMove(m,true,true);
        }
        board.openingTrainingActive = false;
        openingResultController.SetResult(lineIndex, rightCounter, timer);
        rootSelecter.SetOpeningResult();

        Debug.Log("Training END");
    }
    public void ResetTraining(bool restart)
    {
        board.openingTrainingActive = restart;
        lineIndex = 0;
        rightCounter = 0;
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
        }

        currentLine = allLines[lineIndex];


        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + allLines.Count;

        if (!opening.color)
        {
            ManageNext();
        }
    }
}