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
        string from = uci.Substring(0, 2); // Start bei 0, Länge 2 -> "e2"
        string to = uci.Substring(2, 2);
        ChessBoardSceneManager.instance.board.drawOnBoard.drawArrow(BoardUtil.StringToIndex(from), BoardUtil.StringToIndex(to), 2);
    }
}
   