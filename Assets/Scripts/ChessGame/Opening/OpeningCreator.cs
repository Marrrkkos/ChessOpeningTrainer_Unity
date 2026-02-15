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

    public Board dummyBoard;
    public BoardScaler dummyBoardScaler;
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


        foreach(Move move in moves){
            dummyBoard.doMove(move, true);
        }
        if(board.rotation == true)
        {
            dummyBoardScaler.rotate();

        }


        Opening opening = new Opening(name,colorToggle, GameManager.instance.snapPreview.TakePhoto(), moves);
        opening.SaveGame(name);

        

        //reset
        dummyBoard.ResetBoard(true);
        board.ResetBoard(true);
        if(dummyBoard.rotation == true)
        {
            dummyBoardScaler.rotate();

        }

        GameManager.instance.openings.Add(opening);
        GameManager.instance.openingTreesData.openingNames.Add(opening.name);
        GameManager.instance.openingTreesData.Save();

        openingsManager.loadOpenings();
    }

    public void discardOpening()
    {
        board.ResetBoard(true);
        if(dummyBoard.rotation == true)
        {
            dummyBoardScaler.rotate();

        }
    }
}
