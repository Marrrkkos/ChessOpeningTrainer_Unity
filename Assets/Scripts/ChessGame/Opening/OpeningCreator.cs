using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCreator : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    public OpeningsManager openingsManager;
    public InputField nameInput;
    public Image colorImage;

    
    private bool colorToggle = true;
    public void switchColor() {
        if (colorToggle)
        {
            colorImage.color = Color.brown;
            colorToggle = false;
        }
        else {
            colorImage.color = Color.white;
            colorToggle = true;
        }
        boardScaler.rotate();
    }


    public void createOpening() { 
        string name = nameInput.text;
        List<Move> moves = board.currentGame.playedMoves;

        Board dummyBoard = GameManager.instance.dummyBoard;

        foreach(Move move in moves){
            dummyBoard.doMove(move, true);
        }
        Opening opening = new Opening(name,colorToggle, GameManager.instance.snapPreview.TakePhoto(), moves);
        dummyBoard.reset(true);

        GameManager.instance.openings.Add(opening);
        openingsManager.loadOpenings();
    }

}
