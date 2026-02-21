using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SureDelete: MonoBehaviour
{
    public GameObject sureDeleteObject;
    public BoardPreviewLoader boardPreviewLoader;

    public InputField inputField;
    
    private Opening opening;
    public void DeleteOpening()
    {
        
        if(inputField.text != opening.name)
        {
            return;
        }

        GameManager.instance.openings.Remove(opening);
        opening.DeleteOpening(opening.name);

        GameManager.instance.openingTreesData.openingNames.Remove(opening.name);
        GameManager.instance.openingTreesData.Save();

        GameManager.instance.selcetedOpening = null;
        GameManager.instance.selectedMode = "";

        boardPreviewLoader.LoadPreviews();

        sureDeleteObject.SetActive(false);
    }
    public void Cancel()
    {
        sureDeleteObject.SetActive(false);
    }
    public void SetOpeninig(Opening opening)
    {
        this.opening = opening;
    }
}