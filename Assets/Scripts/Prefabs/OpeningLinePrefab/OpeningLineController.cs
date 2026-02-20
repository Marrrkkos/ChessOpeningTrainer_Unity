using UnityEngine;
using UnityEngine.UI;

public class OpeningLineController : MonoBehaviour
{
    public OpeningSliderController openingSliderController;
    public Text sanText;
    private string uci = "";
    public void SetMove(string uci, string san, float whiteWin, float draw, float blackWin)
    {
        sanText.text = san;
        openingSliderController.UpdateSliderColor(whiteWin, draw, blackWin);
        this.uci = uci;
    }


    public void DrawOpeningArrow()
    {
        
    }
}
   