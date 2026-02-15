using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningController : MonoBehaviour
{
    public Board board;
    public Text RemoveLineText;
        

    private string m1 = "";
    private string m2 = "";

    public void DrawOpeningArrows()
    {
        board.drawOnBoard.arrow.ClearArrows();
        List<Move> openingPossibleMoves = board.opening.GetMoves(board.currentGame.playedMoves);
        
        foreach(Move m in openingPossibleMoves)
        {
            board.drawOnBoard.drawArrow(m.from, m.to, 0);
        }
    }



    public void AddToOpening()
    {
        if(board.opening.name != ""){
            board.opening.Add(board.currentGame.playedMoves);
        }
    }
    public void UpdateSelectedArrow(string m1, string m2)
    {
        this.m1 = m1;
        this.m2 = m2;
        RemoveLineText.text = "Are you sure to Remove " + m1 + " to " + m2 + " ?";
    }
    public void HideDelete()
    {
        if(board.opening.name != "")
            board.drawOnBoard.arrow.raycastTarget = false;

        m1 = "";
        m2 = "";
        RemoveLineText.text = "";
        DrawOpeningArrows();
    }
    public void ShowDelete()
    {
        if(board.opening.name != "")
            board.drawOnBoard.arrow.raycastTarget = true;
    }
    public void RemoveFromOpening()
    {
        if(m1 == "" || m2 == "")
        {
            return;
        }
        board.opening.Remove(board.currentGame.playedMoves, BoardUtil.StringToIndex(m1), BoardUtil.StringToIndex(m2));
        board.drawOnBoard.arrow.raycastTarget = false;
        DrawOpeningArrows();
        m1 = "";
        m1 = "";
        RemoveLineText.text = "";
        DrawOpeningArrows();
    }
}