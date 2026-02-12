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
    }
    
    public void DoUndo() {
        Move m = board.undoMove(true);
        //board.currentGame.movesMemory.Add(m);
    }
    public void GoNext() {
        if(board.currentGame.movesMemory.Count == 0){return;}
        board.doMove(board.currentGame.movesMemory.Last(), true);
        //board.currentGame.movesMemory.RemoveAt(board.currentGame.movesMemory.Count - 1);
        
    }
    public void GoEnd() {
        if(board.currentGame.movesMemory.Count == 0){return;}
        int count = board.currentGame.movesMemory.Count;
        for (int i = 0; i < count; i++)
        {
            board.doMove(board.currentGame.movesMemory.Last(), true);
            //board.currentGame.movesMemory.RemoveAt(board.currentGame.movesMemory.Count - 1);
        }
    }
}
