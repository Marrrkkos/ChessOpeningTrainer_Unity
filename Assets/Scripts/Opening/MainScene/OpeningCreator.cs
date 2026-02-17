using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCreator : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    public InputField nameInput;
    public Image colorImage;
    public Board dummyBoard;
    public BoardScaler dummyBoardScaler;

    public BoardPreviewLoader boardPreviewLoader;

    public SnapPreview snapPreview;
    private bool colorToggle = true;
    public void SwitchColor() {
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


    public void CreateOpening() { 
        string name = nameInput.text;
        List<Move> moves = board.currentGame.playedMoves;


        foreach(Move move in moves){
            dummyBoard.doMove(move, true);
        }
        if(board.rotation == true)
        {
            dummyBoardScaler.rotate();

        }


        Opening opening = new Opening(name,colorToggle, snapPreview.TakePhoto(), moves);
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

        boardPreviewLoader.LoadPreviews();
    }
    public void DiscardOpening()
    {
        board.ResetBoard(true);
        if(dummyBoard.rotation == true)
        {
            dummyBoardScaler.rotate();

        }
    }
    public void DeleteOpening()
    {
        board.ResetBoard(true);
        if(dummyBoard.rotation == true)
        {
            dummyBoardScaler.rotate();

        }
    }
}
