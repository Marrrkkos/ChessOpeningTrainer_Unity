using UnityEngine;
using UnityEngine.UI;

public class OpeningCreator : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    public Image colorImage;

    private bool colorToggle = true;
    public void switchColor() {
        if (colorToggle)
        {
            colorImage.color = Color.brown;
        }
        else {
            colorImage.color = Color.white;
        }
        boardScaler.rotate();
    }


    public void createOpening() { 
    
    }

}
