using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardPreview: MonoBehaviour
{
    public RawImage openingPreview;
    public Text openingName;
    private Opening opening;
    public void LoadOpening(Opening opening)
    {
        this.opening = opening;
        openingName.text = opening.name;
        openingPreview.texture = opening.startPos;

    }

    public void GetOpening()
    {
        GameManager.instance.selcetedOpening = opening;
        GameManager.instance.selectedMode = "Opening";
        GameManager.instance.sceneSwitcher.Switch();
    }
}