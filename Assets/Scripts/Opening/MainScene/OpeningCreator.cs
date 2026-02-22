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
    private bool colorToggle = false;
    public void SwitchColor() {
        if (colorToggle)
        {
            colorImage.color = Color.white;
            colorToggle = false;
        }
        else {
            colorImage.color = Color.black;
            colorToggle = true;
        }
        boardScaler.SetRotation(colorToggle);
    }


    public void CreateOpening() { 
        string name = nameInput.text;

        if(name == "")
        {
            DiscardOpening();
            return;
        }

        List<Move> moves = board.currentGame.playedMoves;


        foreach(Move move in moves){
            dummyBoard.doMove(move, true, false);
        }
        dummyBoardScaler.SetRotation(colorToggle);


        Opening opening = new Opening(name,!colorToggle, snapPreview.TakePhoto(), moves);
        opening.SaveGame(name);

        

        //reset
        dummyBoard.ResetBoard(true);
        board.ResetBoard(true);
        dummyBoardScaler.SetRotation(false);

        GameManager.instance.openings.Add(opening);
        GameManager.instance.openingTreesData.openingNames.Add(opening.name);
        GameManager.instance.openingTreesData.Save();

        boardPreviewLoader.LoadPreviews();
    }
    public void DiscardOpening()
    {
        board.ResetBoard(true);
        boardScaler.SetRotation(false);
    }
    public void DeleteOpening()
    {
        board.ResetBoard(true);
        boardScaler.SetRotation(false);

    }
}
