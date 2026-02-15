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

        OpeningsTreesData openingsTreesData = OpeningsTreesData.Load();
        GameManager.instance.openingTreesData = openingsTreesData;
        //Debug.Log(Application.persistentDataPath);
        foreach(string openingString in openingsTreesData.openingNames)
        {
            Opening opening = new();
            opening.LoadGame(openingString);
            GameManager.instance.openings.Add(opening);
            //opening.PrintTreeDepth5();
        }
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
        GameManager.instance.mainBoard.opening = opening;
        GameManager.instance.mainBoard.ResetBoard(true);

        foreach(Move move in opening.moves)
        {
            GameManager.instance.mainBoard.doMove(move, true);
        }
        //if (!opening.color)
        //{
        //    GameManager.instance.mainBoard.mainBoardScaler.rotate();
        //}
    }
}