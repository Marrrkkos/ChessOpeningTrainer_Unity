using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ControllButtons : MonoBehaviour
{
    public Board board;

    public void GoStart() {

        //Instead of undo every Move just add movesMemory from here
        int count = board.currentGame.playedMoves.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            board.currentGame.movesMemory.Add(board.currentGame.playedMoves[i]);
        }

        board.ResetBoard(true);
        if(board.opening.name != ""){
            board.gameController.openingController.DrawOpeningArrows();
        }
    }
    
    public void DoUndo() {
        Move m = board.UndoMove(true, true);
    }
    public void GoNext() {
        if(board.currentGame.movesMemory.Count == 0){return;}
        board.DoMove(board.currentGame.movesMemory.Last(), true, true);
        
    }
    public void GoEnd() {
        if(board.currentGame.movesMemory.Count == 0){return;}
        int count = board.currentGame.movesMemory.Count;
        for (int i = 0; i < count; i++)
        {
            board.DoMove(board.currentGame.movesMemory.Last(), true, true);
        }
    }
}
