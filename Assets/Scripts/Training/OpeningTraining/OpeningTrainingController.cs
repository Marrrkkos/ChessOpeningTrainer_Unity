using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpeningTrainingController : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    private List<List<Move>> allLines = new();
    private int lineIndex = 0;
    private Opening opening;

    private bool color = true;
    private bool ignore = false;

    private List<Move> currentLine = new();
    public void InitTraining(List<List<Move>> allLines, Opening opening)
    {

        board.ResetBoard(true);
        boardScaler.SetRotation(opening.color);

        GameManager.instance.openingTrainingActive = true;
        this.opening = opening;
        this.allLines = allLines;
        color = opening.color;

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
        }

        currentLine = allLines[lineIndex];

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
        GameManager.instance.openingTrainingActive = false;
        Debug.Log("Training END");
    }
}