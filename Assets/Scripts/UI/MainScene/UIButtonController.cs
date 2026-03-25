using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public RectTransform canvas;
    public LayoutElement[] controlButtons;
    public SwipeController swipeController;
    public void LeftButton()
    {
        SetButtons(6,2,2);
        swipeController.ScrollToPage(0);
    }
    public void RightButton()
    {
        SetButtons(2,2,6);
        swipeController.ScrollToPage(2);
    }
    public void MidButton()
    {
        SetButtons(2,6,2);
        swipeController.ScrollToPage(1);
    }
    private void SetButtons(int i, int j, int k)
    {
        controlButtons[0].flexibleWidth = i;
        controlButtons[1].flexibleWidth = j;
        controlButtons[2].flexibleWidth = k;
    }
    public void SetIndex(int i)
    {
        switch (i)
        {
            case 0:
                SetButtons(6,2,2);
                break;
            case 1:
                SetButtons(2,6,2);
                break;
            case 2:
                SetButtons(2,2,6);
                break;    
        }
    }
}
