using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardPreview: MonoBehaviour
{
    public RawImage openingPreview;
    public Text openingName;
    private int openingIndex;
    public void loadOpening(Opening opening, int index)
    {
        openingName.text = opening.name;
        openingPreview.texture = opening.startPos;
        openingIndex = index;
    }

    public void getOpening()
    {
        GameManager.instance.openingsManager.loadOpening(openingIndex);

    }
}