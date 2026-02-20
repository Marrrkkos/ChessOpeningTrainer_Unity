using UnityEngine;
using UnityEngine.UI;

public class OpeningSliderController : MonoBehaviour
{

    public LayoutElement whiteElement;
    public LayoutElement drawElement;
    public LayoutElement blackElement;
    public void UpdateSliderColor(float whiteWin, float drawRate, float blackWin)
    {   
        
        whiteElement.flexibleWidth = whiteWin;
        drawElement.flexibleWidth = drawRate;
        blackElement.flexibleWidth = blackWin;
    }

    
}
