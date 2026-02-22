using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningLoader: MonoBehaviour
{

    
    public Board board;
    public BoardScaler boardScaler;

    public void LoadOpening()
    {
        Opening opening = GameManager.instance.selcetedOpening;
        if(opening.name == ""){return;}

        board.opening = opening;
        board.ResetBoard(true);

        foreach(Move move in opening.moves)
        {
            board.doMove(move, true, false);
        }
        boardScaler.SetRotation(!opening.color);
    }
}