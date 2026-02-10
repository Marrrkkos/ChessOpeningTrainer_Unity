using UnityEngine;
using UnityEngine.UI;

public class ControllButtons : MonoBehaviour
{
    public Board board;
    public void start() {
        board.reset();
    }
    public void next() {



        if(board.currentGame.loadedGame.Count != 0)  // Game is currently Loaded
        {


            board.doSimpleMove(board.currentGame.loadedGame[board.currentGame.loadedGameMovesIndex], true);
            board.currentGame.loadedGameMovesIndex++;
            return;
        }

    }
    public void undo() {
        GameManager.instance.mainBoard.undoMove(true);
    }
    public void end() {
        Game game = GameManager.instance.mainBoard.currentGame;
        //game.simpleMovesIndex = 0;
        GameManager.instance.mainBoard.reset();
        GameManager.instance.gameIndex++;
    }
}
