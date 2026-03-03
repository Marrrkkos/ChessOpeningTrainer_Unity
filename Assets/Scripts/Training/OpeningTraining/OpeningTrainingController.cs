using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TrainingMode
{
    Normal = 0,
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

    private Node currentNode = new();
    private List<Node> queue = new();
    private int openingSize = 0;
    
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

        this.opening = opening;
        this.depth = depth;
        this.mode = mode;

        board.openingTrainingActive = true;
        openingSize = opening.GetOpeningSize();
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
            board.doMove(m, true, true);
            currentNode = currentNode.children[0];
        }

        if(currentNode.children.Count == 0){return;}

        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingSize;


        foreach(Node n in currentNode.children)
        {
            if(n.children.Count >= 0)
                queue.Add(n);
        }
        GoNextLine();
        CalcNewTrys();
        //ManageNext();
        
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

        Debug.Log("Trys For Next Turn: " + trys);
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
    private bool MovesLeft()
    {
        if(currentNode.children.Count == 0 || board.currentGame.playedMoves.Count >= depth){

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
            board.doMove(m, true, true);
            currentNode = currentNode.children[0];
        }



        percentNumber.text = "-";
        time.text = "00:00";
        openingName.text = opening.name;
        possibleLines.text = "0/" + openingSize;

        foreach(Node n in currentNode.children)
        {
            if(n.children.Count >= 0)
                queue.Add(n);
        }
        GoNextLine();
        CalcNewTrys();
    }
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Wähle einen zufälligen Index von 0 bis i
            int randomIndex = Random.Range(0, i + 1);

            // Tausche die Elemente
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}