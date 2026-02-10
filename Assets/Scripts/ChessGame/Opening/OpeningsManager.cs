using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningsManager: MonoBehaviour
{
    public List<BoardPreview> boardPreviews;
    public void loadOpenings()
    {
        for (int i = 0; i < GameManager.instance.openings.Count; i++)
        {
            boardPreviews[i].loadOpening(GameManager.instance.openings[i]);
        }
        
    }
}