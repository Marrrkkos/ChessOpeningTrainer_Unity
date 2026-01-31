using UnityEngine;
using UnityEngine.UI;

public class ControllButtons : MonoBehaviour
{
    public RectTransform boardRect;
    int currentRotation = 0;
    public void rotate() {

        if (currentRotation == 0)
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 180);
            GameManager.instance.mainBoard.rotation = true;
            currentRotation = 180;
        }
        else if(currentRotation == 180)
        {
            boardRect.localEulerAngles = new Vector3(0, 0, 0);
            GameManager.instance.mainBoard.rotation = false;
            currentRotation = 0;
        }
        rotatePieces(currentRotation);
    }
    private void rotatePieces(int rotation) {
        Field[] fields = GameManager.instance.mainBoard.fields;
        for (int i = 0; i < 64; i++) {
            fields[i].pieceImage.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotation);
        }
    }
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
