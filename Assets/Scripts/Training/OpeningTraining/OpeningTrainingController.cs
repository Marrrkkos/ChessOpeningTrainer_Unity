using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpeningTrainingController : MonoBehaviour
{
    [Header("Board")]
    public Board board;
    public BoardScaler boardScaler;

    [Header("TrainingPanel")]
    public Text wrongNumber;
    public Text rightNumber;
    public Text percentNumber;
    public Text time;
    public Text openingName;
    public Text possibleLines;

    [Header("OpeningResult")]
    public OpeningResultController openingResultController;
    private List<List<Move>> allLines = new();
    private int lineIndex = 0;
    private Opening opening;
    private List<Move> currentLine = new();
    public void InitTraining(List<List<Move>> allLines, Opening opening)
    {
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(opening.color);

        this.opening = opening;
        this.allLines = allLines;

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
        }

        currentLine = allLines[lineIndex];

        wrongNumber.text = "0";
        rightNumber.text = "0";
        percentNumber.text = "100%";
        time.text = "00:00";
        openingName.text = opening.name;


        board.openingTrainingActive = true;
        if (!opening.color)
        {
            ManageNext();
        }


    }
    public void ManageNext()
    {

        if (board.currentGame.playedMoves.Last().Equals(currentLine[board.currentGame.playedMoves.Count - 1]))
        {
            Debug.Log("Right Move");
        }
        else
        {
            Debug.Log("Wrong Move");
            GoNextLine();
            return;
        }


            if(currentLine.Count > board.currentGame.playedMoves.Count){
                board.doMove(currentLine[board.currentGame.playedMoves.Count], true, true);
            }
            else
            {
                GoNextLine();
            }

    }

    private void GoNextLine()
    {
        lineIndex++;
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
    }

    private void EndTraining()
    {
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.doMove(m,true,true);
        }
        board.openingTrainingActive = false;


        Debug.Log("Training END");
    }
}