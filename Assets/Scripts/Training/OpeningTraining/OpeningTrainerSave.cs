using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class OpeningTrainingControllerSave : MonoBehaviour
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

    private int openingSize = 0;
    
    private int lineCounter;
    private float rightCounter;
    private float timer;


    //Constructor
    private Opening opening;
    private int depth;
    private bool goNextOnWrongMove;
    private bool shuffle;
    private bool showRightAfterMiss;
    private bool startLinesFromStart;
    public void InitTraining(Opening opening, int depth, bool goNextOnWrongMove, bool shuffle, bool showRightAfterMiss, bool startLinesFromStart)
    {
        if(opening.rootNode.children.Count == 0){rootSelecter.SetOpening(); return; }

        this.opening = opening;
        this.depth = depth;
        this.goNextOnWrongMove = goNextOnWrongMove;
        this.shuffle = shuffle;
        this.showRightAfterMiss = showRightAfterMiss;
        this.startLinesFromStart = startLinesFromStart;


        board.openingTrainingActive = true;
        //openingSize = opening.GetOpeningSize();
        Debug.Log("openingSize: " + openingSize);
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
            board.doMove(m, true,false, true);
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
    int childrenCount = 0;
    public void ManageNext()
    {
        // CheckForCorrectMove()
        // HandleMultiplePossibles()
        // 
        //

        bool correctMove = CheckForCorrectMove();
        if(childrenCount != 0)
        {   
            childrenCount--;
            return;
        }


        if (!correctMove)
        {   
            GoNextLine(false); 
            return;
        }

        if(!MovesLeft()){
            
            GoNextLine(true); 
            return;
        }

        DoNextBotMove();

        if(!MovesLeft()){

            GoNextLine(true); 
            return;
        }

        if (shuffle)
        {
            //Shuffle(queue);
        }

        InitNextPossibles();
        

    }
    private void InitNextPossibles()
    {
        foreach(Node n in currentNode.children)
        {   
            currentNodeSave.Add(n);
            childrenCount++;
        }
    }
    private bool MovesLeft()
    {
        if(currentNode.children.Count == 0 || board.currentGame.playedMoves.Count >= depth){

            return false;
        }
        return true;
    }
    private bool CheckForCorrectMove()
    {
        bool match = false;
        Node matchedNode = new();
        foreach(Node n in currentNode.children)
        {   
            if (n.move.Equals(board.currentGame.playedMoves.Last()))
            {
                // Good Move
                match = true;
                matchedNode = n;
                //currentNode = n;
            }
        }
        
        

        if (match)
        {   
            foreach(Node n1 in matchedNode.children)
            {
                Debug.Log("Added " + n1.move.ToString() + " to the Queue");
                queue.Add(n1);
            }
            currentNodeSave.Remove(matchedNode);
            return true;
        }
        else
        {
            if (!goNextOnWrongMove)
            {
                foreach(Node n1 in matchedNode.children)
                {
                    Debug.Log("Added " + n1.move.ToString() + " to the Queue");
                    queue.Add(n1);
                }
            }
            return false;
        }

    }
    private void DoNextBotMove()
    {
        //Get Next Move for Bot
        for(int i = 0; i < currentNode.children.Count; i++)
        {
            if (i != 0)
            {
                Debug.Log("Added " + currentNode.children[i].move.ToString() + " to the Queue");
                queue.Add(currentNode.children[i]);
            }
        }
        currentNode = currentNode.children[0];

        Debug.Log(currentNode.move.ToString());
        board.doMove(currentNode.move, true,false, true);
    }

    private void GoNextLine(bool everythingRight)
    {   
        lineCounter++;
        if(everythingRight)
        {
            rightCounter++;
        }

        if(queue.Count == 0)
        {
            EndTraining();
            return;
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
            board.doMove(movesTillNode[i],true,false,true);
        }
        

        currentNode = queue.First(); 
        queue.RemoveAt(0);

        float percent = (rightCounter / lineCounter) * 100;
        percentNumber.text = percent + "%";
        possibleLines.text = lineCounter + "/" + openingSize;

        //DoNextBotMove();
    }

    private void EndTraining()
    {
        Debug.Log("End Trainig");
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.doMove(m,true,false,true);
        }
        board.openingTrainingActive = false;
        openingResultController.SetResult(lineCounter, rightCounter, timer);
        rootSelecter.SetOpeningResult();

        Debug.Log("Training END");
    }
    public void ResetTraining(bool restart)
    {
        board.openingTrainingActive = restart;
        currentNode = opening.rootNode;
        lineCounter = 0;
        rightCounter = 0;
        board.drawOnBoard.arrow.ClearAllArrows();
        board.ResetBoard(true);
        boardScaler.SetRotation(!opening.color);

        foreach(Move m in opening.moves)
        {
            board.doMove(m, true,false, true);
            currentNode = currentNode.children[0];
        }



        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingSize;

        DoNextBotMove();
    }
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}