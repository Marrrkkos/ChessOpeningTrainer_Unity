using UnityEngine;
using UnityEngine.UI;

public class ControllButtons : MonoBehaviour
{
    public Board board;
    public void GoStart() {
        board.ResetBoard(true);
    }
    public void GoNext() {



        if(board.currentGame.loadedGame.Count != 0)  // Game is currently Loaded
        {


            board.doSimpleMove(board.currentGame.loadedGame[board.currentGame.loadedGameMovesIndex], true);
            board.currentGame.loadedGameMovesIndex++;
            return;
        }

    }
    public void DoUndo() {
        GameManager.instance.mainBoard.undoMove(true);
    }
    public void GoEnd() {
        Game game = GameManager.instance.mainBoard.currentGame;
        //game.simpleMovesIndex = 0;
        GameManager.instance.mainBoard.ResetBoard(true);
        GameManager.instance.gameIndex++;
    }
}
