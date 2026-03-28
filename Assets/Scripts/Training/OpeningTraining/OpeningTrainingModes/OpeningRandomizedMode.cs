using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpeningRandomizedMode : MonoBehaviour
{
    public Board board;
    public RootSelecter rootSelecter;




    private List<Node> allPossibleNodes = new();
    private Node currentNode = new();
    private int trys;
    private List<Node> currentNodeSave = new();
    private Opening opening;
    public void InitTraining(Opening opening, int depth)
    {
        allPossibleNodes = opening.GetAllNodes();
        this.opening = opening;

        GoNextLine();
        CalcNewTrys();
    }

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
    private bool CheckPlayerMove()
    {
        bool match = false;
        foreach(Node n in currentNode.children)
        {
            if (n.move.Equals(board.currentGame.playedMoves.Last()))
            {   
                currentNodeSave.Remove(n);
                match = true;
            }
        }

        trys--;
        board.UndoMove(true, false);
        
        if(trys == 0)
        {
            return false;
        }
        return true;
    }
    public void ResetTraining()
    {
        //currentNode = opening.rootNode;
        //lineCounter = 0;
        //rightCounter = 0;
        allPossibleNodes = opening.GetAllNodes();

        GoNextLine();
        CalcNewTrys();
    }

    private void GoNextLine()
    {
        if(allPossibleNodes.Count == 0)
        {
            EndTraining();
            return;
        }

        Node n = allPossibleNodes.Last();

        currentNode = n;
        allPossibleNodes.RemoveAt(allPossibleNodes.Count-1);

        board.ResetBoard(true);

        List<Move> movesTillNode = new();
        while(n.parentNode != null)
        {
            movesTillNode.Add(n.move);
            n = n.parentNode;
        }
        for (int i = movesTillNode.Count - 1; i >= 0; i--)
        {
            board.DoMove(movesTillNode[i],true, false);
        }
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
    private void EndTraining()
    {
        Debug.Log("End Trainig");
        board.ResetBoard(true);
        foreach(Move m in opening.moves)
        {
            board.DoMove(m,true, false);
        }
        board.gameController.openingTrainingActive = false;
        //openingResultController.SetResult(lineCounter, rightCounter, timer);
        rootSelecter.SetOpeningResult();

        Debug.Log("Training END");
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