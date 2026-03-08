using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpeningTestingMode : MonoBehaviour
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
    public void InitTraining(Opening opening, int depth)
    {

        if(opening.rootNode.children.Count == 0){rootSelecter.SetOpening(); return; }

        this.opening = opening;
        this.depth = depth;

        openingLeafSize = opening.GetNodeLeafSize(opening.rootNode);
        openingNodeSize = opening.GetNodeMovesSize(opening.rootNode);

        lineCounter = 0;
        rightCounter = 0;
        timer = 0;

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
        possibleLines.text = "0/" + openingLeafSize;


        foreach(Node n in currentNode.children)
        {
            if(n.children.Count >= 0)
                queue.Add(n);
        }
        GoNextLine();
        CalcNewTrys();
        
    }
    List<Node> currentNodeSave = new();
    int trys = 0;
    public void ManageNext()
    {
        if(trys > 0)
        {
            bool movesLeft = CheckPlayerMove();

            if (movesLeft)
            {
                return;
            }
            else
            {   
                currentNodeSave.Clear();
                GoNextLine();
            }
        }
        //DoNextBotMove();
        CalcNewTrys();
    }
    private void CalcNewTrys()
    {
        trys = currentNode.children.Count;

        foreach(Node n in currentNode.children)
        {
            currentNodeSave.Add(n);
        }
        if(trys == 0)
        {
            GoNextLine();
        }
    }
    private bool CheckPlayerMove()  // returns true if moves are left
    {
        bool match = false;
        foreach(Node n in currentNode.children)
        {
            if (n.move.Equals(board.currentGame.playedMoves.Last()))
            {   
                foreach(Node child in n.children)
                {   
                    if(child.children.Count != 0)
                        queue.Add(child);
                }

                currentNodeSave.Remove(n);
                match = true;
            }
        }

        trys--;
        board.undoMove(true);
        
        if(trys == 0)
        {
            return false;
        }
        return true;
    }

    private void GoNextLine()
    {   
        if(queue.Count == 0)
        {
            EndTraining();
            return;
        }

        Node n = queue.Last();

        currentNode = n;
        queue.RemoveAt(queue.Count-1);

        board.ResetBoard(true);

        List<Move> movesTillNode = new();
        while(n.parentNode != null)
        {
            movesTillNode.Add(n.move);
            n = n.parentNode;
        }
        for (int i = movesTillNode.Count - 1; i >= 0; i--)
        {
            board.doMove(movesTillNode[i],true,true);
        }

    }

    private void EndTraining()
    {
        Debug.Log("End Trainig");
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
    public void ResetTraining()
    {
        
        currentNode = opening.rootNode;
        lineCounter = 0;
        rightCounter = 0;

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true, true);
            currentNode = currentNode.children[0];
        }



        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingLeafSize;

        foreach(Node n in currentNode.children)
        {
            if(n.children.Count >= 0)
                queue.Add(n);
        }
        GoNextLine();
        CalcNewTrys();
    }
}