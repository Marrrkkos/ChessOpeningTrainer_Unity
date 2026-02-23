using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class BoardPreview: MonoBehaviour
{
    public RawImage openingPreview;
    public Text openingName;
    public UISquircle color;
    public Text sizeText;
    private Opening opening;
    public void LoadOpening(Opening opening)
    {
        this.opening = opening;
        openingName.text = opening.name;
        openingPreview.texture = opening.startPos;
        color.color = opening.color ? Color.white : Color.black;
        sizeText.text = "200";

    }
    public void ShowDelete()
    {
        MainSceneManager.instance.sureDelete.SetOpeninig(opening);
        MainSceneManager.instance.sureDelete.gameObject.SetActive(true);
    }
    public void GetOpening()
    {
        Debug.Log("Load Opening; " + opening.name);
        GameManager.instance.selcetedOpening = opening;
        GameManager.instance.selectedMode = "Opening";
        GameManager.instance.sceneSwitcher.Switch();
    }
}