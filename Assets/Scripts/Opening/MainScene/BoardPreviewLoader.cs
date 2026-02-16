using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardPreviewLoader : MonoBehaviour{
public List<BoardPreview> boardPreviews;

    public void Start()
    {

        OpeningTreesData openingsTreesData = OpeningTreesData.Load();
        GameManager.instance.openingTreesData = openingsTreesData;
        //Debug.Log(Application.persistentDataPath);

        bool containsAlready = false;
        foreach(string openingString in openingsTreesData.openingNames)
        {
            foreach(Opening o in GameManager.instance.openings)
            {
                if(o.name == openingString)
                {
                    containsAlready = true;
                }
            }

            if(!containsAlready){
                Opening opening = new();
                opening.LoadGame(openingString);
                GameManager.instance.openings.Add(opening);
            }
            containsAlready = false;
            //opening.PrintTreeDepth5();
        }
        LoadPreviews();


    }
    public void LoadPreviews()
    {
        for (int i = 0; i < GameManager.instance.openings.Count; i++)
        {
            boardPreviews[i].LoadOpening(GameManager.instance.openings[i]);
            boardPreviews[i].gameObject.SetActive(true);

        }
    }
}