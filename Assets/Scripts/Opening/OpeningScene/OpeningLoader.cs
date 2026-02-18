using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningLoader: MonoBehaviour
{

    
    public Board board;
    public BoardScaler boardScaler;

    public void LoadOpening()
    {
        string selcetedOpening = GameManager.instance.selcetedOpening;
        if(selcetedOpening == "")
            return;

        Opening opening = new Opening();
        foreach(Opening o in GameManager.instance.openings)
        {
            if(o.name == selcetedOpening){ opening = o;}
        }
        board.opening = opening;
        board.ResetBoard(true);

        foreach(Move move in opening.moves)
        {
            board.doMove(move, true);
        }
        if (!opening.color)
        {
            boardScaler.rotate();
        }
    }
}