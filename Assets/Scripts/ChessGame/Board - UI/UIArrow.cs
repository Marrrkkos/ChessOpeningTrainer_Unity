using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasRenderer))]
public class UIArrow : MaskableGraphic, IPointerClickHandler
{
    [System.Serializable]
    public struct ArrowData
    {
        public string from;
        public string to;
        public Vector2 start;
        public Vector2 end;
        public Color color;
        public int listIndex;
    }

    public List<ArrowData> activeOpeningArrows = new ();
    public List<ArrowData> activeEngineArrows = new ();
    public List<ArrowData> activeOpeningBookArrows = new();

    public float shaftWidth = 10f;
    public float headSize = 30f;

    public OpeningController openingController;
    private int lastSelectedIndex = -1;

    public void AddArrow(string f1, string f2, Vector2 from1, Vector2 to1, Color col, int index)
    {
        switch (index)
        {
            case 0:
                activeOpeningArrows.Add(new ArrowData {from = f1, to = f2, start = from1, end = to1, color = col, listIndex = index});
                break;
            case 1:
                activeEngineArrows.Add(new ArrowData {from = f1, to = f2, start = from1, end = to1, color = col, listIndex = index});
                break;
            case 2:
                if(activeOpeningBookArrows.Count == 1)
                    activeOpeningBookArrows[0] = new ArrowData {from = f1, to = f2, start = from1, end = to1, color = col, listIndex = index};
                    else
                        activeOpeningBookArrows.Add(new ArrowData {from = f1, to = f2, start = from1, end = to1, color = col, listIndex = index});
                break;
        }
        
        SetVerticesDirty();
    }
    public void ClearArrows(int index)
    {
        switch (index)
        {
            case 0:
                activeOpeningArrows.Clear();
                break;
            case 1:
                activeEngineArrows.Clear();
                break;
            case 2:
                activeOpeningBookArrows.Clear();
                break;
        }
        SetVerticesDirty();
    }
    public void ClearAllArrows()
    {
        activeOpeningArrows.Clear();
        activeEngineArrows.Clear();
        activeOpeningBookArrows.Clear();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

    List<int> hitIndices = new List<int>();
    for (int i = 0; i < activeOpeningArrows.Count; i++)
    {
        if(activeOpeningArrows[i].listIndex != 0){continue;}
        if (IsPointNearArrow(localPoint, activeOpeningArrows[i]))
        {
            hitIndices.Add(i);
        }
    }

    if (hitIndices.Count == 0) return;

    int nextIndex;

    if (hitIndices.Contains(lastSelectedIndex))
    {
        int currentHitListIndex = hitIndices.IndexOf(lastSelectedIndex);
        int nextHitListIndex = (currentHitListIndex + 1) % hitIndices.Count;
        nextIndex = hitIndices[nextHitListIndex];
    }
    else
    {
        nextIndex = hitIndices[0];
    }
    lastSelectedIndex = nextIndex;
    SelectArrow(nextIndex);
    }
    private void RepaintArrows()
    {
        for(int i = 0; i < activeOpeningArrows.Count; i++)
        {
            if(activeOpeningArrows[i].listIndex == 0)
            {
                ArrowData arrow = activeOpeningArrows[i];
                arrow.color = Color.lightGreen;
                activeOpeningArrows[i] = arrow;
            }
        }
    }
private void SelectArrow(int index)
{
    RepaintArrows();
    ArrowData arrow = activeOpeningArrows[index];
    Debug.Log($"Pfeil {index} ausgewählt! Von {arrow.start} nach {arrow.end}");
    arrow.color = Color.red;
    activeOpeningArrows[index] = arrow;
    openingController.UpdateSelectedArrow(arrow.from, arrow.to);
    MoveToFront(index);
}
private void MoveToFront(int index)
{
    ArrowData selected = activeOpeningArrows[index];
    activeOpeningArrows.RemoveAt(index);
    activeOpeningArrows.Add(selected);
    lastSelectedIndex = activeOpeningArrows.Count - 1;
    SetVerticesDirty();
}
private bool IsPointNearArrow(Vector2 point, ArrowData arrow)
{
    Vector2 lineVec = arrow.end - arrow.start;
    float lineLen = lineVec.magnitude;
    Vector2 lineDir = lineVec.normalized;

    // 1. Projektion des Punktes auf die Linie (Skalarprodukt)
    // Wie weit ist der Punkt entlang der Linie vom Startpunkt entfernt?
    float projection = Vector2.Dot(point - arrow.start, lineDir);

    // Prüfen, ob der Klick innerhalb der Länge des Pfeils liegt
    if (projection < 0 || projection > lineLen) return false;

    // 2. Senkrechter Abstand zur Linie berechnen
    Vector2 closestPointOnLine = arrow.start + lineDir * projection;
    float distanceToLine = Vector2.Distance(point, closestPointOnLine);

    // Klick ist gültig, wenn er nah genug am Schaft ist (Toleranz = shaftWidth)
    // Oder wir prüfen die Pfeilspitze separat:
    if (projection > lineLen - headSize) 
    {
        // In der Nähe der Spitze darf man etwas weiter weg klicken (Breite der Spitze)
        return distanceToLine < headSize / 1.5f;
    }
    
    return distanceToLine < shaftWidth * 1.5f; // 1.5x Puffer für bessere Bedienbarkeit
}


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        foreach (var arrow in activeEngineArrows)
        {
            DrawSingleArrow(vh, arrow);
        }
        foreach (var arrow in activeOpeningBookArrows)
        {
            DrawSingleArrow(vh, arrow);
        }
        foreach (var arrow in activeOpeningArrows)
        {
            DrawSingleArrow(vh, arrow);
        }
    }

    private void DrawSingleArrow(VertexHelper vh, ArrowData data)
    {
        Vector2 direction = data.end - data.start;
        if (direction.magnitude < 0.1f) return;

        direction.Normalize();
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Vector2 headBaseCenter = data.end - (direction * headSize);

        // Schaft-Punkte
        Vector2 v1 = data.start + (perpendicular * shaftWidth / 2f);
        Vector2 v2 = data.start - (perpendicular * shaftWidth / 2f);
        Vector2 v3 = headBaseCenter + (perpendicular * shaftWidth / 2f);
        Vector2 v4 = headBaseCenter - (perpendicular * shaftWidth / 2f);

        AddQuad(vh, v1, v2, v3, v4, data.color);

        // Spitzen-Punkte
        Vector2 t1 = data.end;
        Vector2 t2 = headBaseCenter + (perpendicular * headSize / 2f);
        Vector2 t3 = headBaseCenter - (perpendicular * headSize / 2f);

        AddTriangle(vh, t1, t2, t3, data.color);
    }

    // Hilfsfunktion: F�gt ein Viereck zum Mesh hinzu
    private void AddQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color)
    {
        int index = vh.currentVertCount;

        UIVertex vert = UIVertex.simpleVert;
        vert.color = color; // Nutzt die Farbe aus dem Inspector

        vert.position = v1; vh.AddVert(vert);
        vert.position = v2; vh.AddVert(vert);
        vert.position = v3; vh.AddVert(vert);
        vert.position = v4; vh.AddVert(vert);

        vh.AddTriangle(index + 0, index + 1, index + 2);
        vh.AddTriangle(index + 2, index + 1, index + 3);
    }

    // Hilfsfunktion: F�gt ein Dreieck zum Mesh hinzu
    private void AddTriangle(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
    {
        int index = vh.currentVertCount;

        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;

        vert.position = v1; vh.AddVert(vert);
        vert.position = v2; vh.AddVert(vert);
        vert.position = v3; vh.AddVert(vert);

        vh.AddTriangle(index + 0, index + 1, index + 2);
    }
}
