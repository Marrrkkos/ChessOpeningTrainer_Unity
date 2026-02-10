using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardPreview: MonoBehaviour
{
    public RawImage openingPreview;
    public Text openingName;
    public void loadOpening(Opening opening)
    {
        openingName.text = opening.name;
        openingPreview.texture = opening.startPos;

    }
}