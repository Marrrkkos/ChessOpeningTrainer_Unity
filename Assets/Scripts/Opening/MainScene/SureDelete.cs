using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SureDelete: MonoBehaviour
{
    public BoardPreviewLoader boardPreviewLoader;

    public InputField inputField;
    public Text sureText;
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

        inputField.text = "";
        boardPreviewLoader.LoadPreviews();
    }
    public void Cancel()
    {
        inputField.text = "";
    }
    public void SetOpeninig(Opening opening)
    {
        this.opening = opening;
        sureText.text = "Are you Sure you want to Delete this Opening: " + opening.name;
    }
}