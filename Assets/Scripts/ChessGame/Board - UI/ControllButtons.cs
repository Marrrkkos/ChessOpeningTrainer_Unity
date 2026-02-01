using UnityEngine;
using UnityEngine.UI;

public class ControllButtons : MonoBehaviour
{
    
    public void start() {
        GameManager.instance.mainBoard.reset();
    }
    public void next() {
        Game game = GameManager.instance.gameDataBase[GameManager.instance.gameIndex];
        GameManager.instance.mainBoard.doSimpleMove(game.simpleMoves[game.simpleMovesIndex], true);
        game.simpleMovesIndex++;
        if (game.simpleMoves.Count > game.moves.Count) { 
        
        }
    }
    public void undo() {
        GameManager.instance.mainBoard.undoMove(true);
    }
    public void end() {
        Game game = GameManager.instance.mainBoard.currentGame;
        game.simpleMovesIndex = 0;
        GameManager.instance.mainBoard.reset();
        GameManager.instance.gameIndex++;
    }
}
