using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardPreview: MonoBehaviour
{
    public RawImage openingPreview;
    public Text openingName;
    public void LoadOpening(Opening opening)
    {
        openingName.text = opening.name;
        openingPreview.texture = opening.startPos;

    }

    public void GetOpening()
    {
        GameManager.instance.selcetedOpening = openingName.text;
        GameManager.instance.selectedMode = "Opening";
        GameManager.instance.sceneSwitcher.Switch();
    }
}