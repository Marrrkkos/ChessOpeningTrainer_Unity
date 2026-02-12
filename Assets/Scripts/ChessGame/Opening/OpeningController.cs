using System.Collections.Generic;
using UnityEngine;

public class OpeningController : MonoBehaviour
{
    public Board board;
    public void ClearOpeningArrows()
    {
        board.drawOnBoard.ClearArrows();
    }

    public void DrawOpeningArrows()
    {
        Opening opening = board.opening;
        List<Move> openingPossibleMoves = opening.GetMoves(board.currentGame.playedMoves);

        foreach(Move m in openingPossibleMoves)
        {
            board.drawOnBoard.drawArrow(m.from, m.to);
        }
    }



    public void AddToOpening()
    {
        Opening opening = board.opening;
        opening.add(board.currentGame.playedMoves);
    }
    
    public void RemoveFromOpening()
    {
        Opening opening = board.opening;
        opening.remove(board.currentGame.playedMoves);
    }
}