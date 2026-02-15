using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningsManager: MonoBehaviour
{

    
    public List<BoardPreview> boardPreviews;
    public Board mainBoard;
    public BoardScaler mainBoardScaler;

    public void Start()
    {
        //Debug.Log(Application.persistentDataPath);
        bool x = true;
        int i = 0;
        //while (x)
        //{
            Opening opening = new Opening(i);
            x = opening.LoadGame(i);

            if (x)
            {
                GameManager.instance.openings.Add(opening);
                i++;
                opening.PrintTreeDepth5();
            }
        //}
        loadOpenings();


    }
    public void loadOpenings()
    {
        for (int i = 0; i < GameManager.instance.openings.Count; i++)
        {
            boardPreviews[i].loadOpening(GameManager.instance.openings[i], i);
            boardPreviews[i].gameObject.SetActive(true);

        }
        
  
    }

    public void loadOpening(int openingIndex)
    {
        Opening opening = GameManager.instance.openings[openingIndex];
        mainBoard.opening = opening;
        mainBoard.ResetBoard(true);

        foreach(Move move in opening.moves)
        {
            mainBoard.doMove(move, true);
        }
        if (!opening.color)
        {
            mainBoardScaler.rotate();
        }
    }
}