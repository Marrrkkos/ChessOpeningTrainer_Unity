using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpeningTrainingController1 : MonoBehaviour
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
    private Opening opening;

    private Node currentNode = new();
    private List<Node> queue = new();

    private int openingSize = 0;
    
    private int lineCounter;
    private float rightCounter;
    private float timer;
    public void InitTraining(Opening opening)
    {
        if(opening.rootNode.children.Count == 0){rootSelecter.SetOpening(); return; }

        board.openingTrainingActive = true;
        openingSize = opening.GetOpeningSize();
        lineCounter = 0;
        rightCounter = 0;
        timer = 0;
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        this.opening = opening;
        currentNode = opening.rootNode;

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
            currentNode = currentNode.children[0];
        }

        if(currentNode.children.Count == 0){return;}

        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingSize;

        DoNextBotMove();


    }
    List<Node> currentNodeSave = new();
    public void ManageNext()
    {
        bool correctMove = CheckForCorrectMove();
        if (!correctMove)
        {   

            if(queue.Count == 0)
            {
                EndTraining();
                return;
            }
            GoNextLine(false);
            return;
        }

        if(!MovesLeft()){
            if(queue.Count == 0)
            {
                EndTraining();
                return;
            }
            GoNextLine(true); 
            return;
        }

        DoNextBotMove();

        if(!MovesLeft()){

            if(queue.Count == 0)
            {
                EndTraining();
                return;
            }
            GoNextLine(true); 
            return;
        }
        
    }
    private bool MovesLeft()
    {
        if(currentNode.children.Count == 0){

            return false;
        }
        return true;
    }
    private bool CheckForCorrectMove()
    {
        
        //Debug.Log("Current Node Move: " + currentNode.move.ToString());
        

        //Check for NextLine/End
        

        //Check For Match
        bool match = false;

        foreach(Node n in currentNode.children)
        {   
            if (n.move.Equals(board.currentGame.playedMoves.Last()))
            {
                // Good Move
                match = true;
                currentNode = n;
            }
            else
            {
                currentNodeSave.Add(n);
            }
        }

        if (match)
        {
            queue.AddRange(currentNodeSave);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DoNextBotMove()
    {
        //Get Next Move for Boot
        for(int i = 0; i < currentNode.children.Count; i++)
        {
            if (i != 0)
            {
                queue.Add(currentNode.children[i]);
            }
        }
        currentNode = currentNode.children[0];


        board.doMove(currentNode.move, true, true);
    }

    private void GoNextLine(bool everythingRight)
    {
        
        if(everythingRight)
        {
            rightCounter++;
        }

        board.ResetBoard(true);
        
        Node n = queue.First();
        List<Move> movesTillNode = new();
        while(n.parentNode != null)
        {
            movesTillNode.Add(n.move);
            n = n.parentNode;
        }
        for (int i = movesTillNode.Count - 1; i >= 0; i--)
        {
            Debug.Log("MovesTillNode: " + movesTillNode[i].ToString());
            board.doMove(movesTillNode[i],true,true);
        }
        //foreach(Move m in movesTillNode)
        //{
        //    Debug.Log("MovesTillNode: " + m.ToString());
        //    board.doMove(m,true,true);
        //}

        currentNode = queue.First(); 
        queue.RemoveAt(0);

        float percent = (rightCounter / lineCounter) * 100;
        percentNumber.text = percent + "%";
        possibleLines.text = lineCounter + "/" + openingSize;

        //DoNextBotMove();
    }

    private void EndTraining()
    {
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.doMove(m,true,true);
        }
        board.openingTrainingActive = false;
        openingResultController.SetResult(lineCounter, rightCounter, timer);
        rootSelecter.SetOpeningResult();

        Debug.Log("Training END");
    }
    public void ResetTraining(bool restart)
    {
        board.openingTrainingActive = restart;
        lineCounter = 0;
        rightCounter = 0;
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
        }



        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingSize;

        if (!opening.color)
        {
            ManageNext();
        }
    }
}