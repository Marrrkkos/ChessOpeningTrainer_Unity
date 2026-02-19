using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardPreviewLoader : MonoBehaviour{
    public List<BoardPreview> boardPreviews;
    public Board dummyBoard;
    public BoardScaler dummyBoardScaler;
    public SnapPreview snapPreview;
    public void Start()
    {

        OpeningTreesData openingsTreesData = OpeningTreesData.Load();
        GameManager.instance.openingTreesData = openingsTreesData;
        //Debug.Log(Application.persistentDataPath);

        foreach(string openingString in openingsTreesData.openingNames)
        {
            Opening opening = new();
            opening.LoadGame(openingString);
            GameManager.instance.openings.Add(opening);
            //opening.PrintTreeDepth5();
        }
        LoadPreviews();

    }
    public void LoadPreviews()
    {
        for (int i = 0; i < GameManager.instance.openings.Count; i++)
        {
            Opening opening = GameManager.instance.openings[i];
            foreach(Move move in opening.moves){
                dummyBoard.doMove(move, true, false);
            }
            if(!opening.color)
            {   
                dummyBoardScaler.SetRotation(false);
            }
            opening.startPos = snapPreview.TakePhoto();

            boardPreviews[i].LoadOpening(opening);
            boardPreviews[i].gameObject.SetActive(true);

            dummyBoard.ResetBoard(true);
        }
    }
}