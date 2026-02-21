using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class AllPanelScaler : MonoBehaviour
{
    public float offSet;
    public float controlBarOffSet;


    public List<RectTransform> panels;
    public panel2Scaler panel2Scaler;
    public RectTransform canvas;

    void Awake() {
        //setPanelSize();
    }

    private void setPanelSize() {
        float width = canvas.rect.width;
        float height = canvas.rect.height;

        Vector2 panelSize = new Vector2(width + offSet, height - controlBarOffSet);
        Vector2 contentSize = new Vector2(width, height);


        foreach (RectTransform panel in panels)
        {

            panel.sizeDelta = panelSize;
            RectTransform content = panel.GetChild(0) as RectTransform;
            content.sizeDelta = contentSize;
        }
        panel2Scaler.ScalePanel2(panelSize.x);
    }
}
