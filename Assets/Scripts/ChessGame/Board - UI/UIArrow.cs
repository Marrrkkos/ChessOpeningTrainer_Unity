using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIArrow : MaskableGraphic
{
    [System.Serializable]
    public struct ArrowData
    {
        public Vector2 start;
        public Vector2 end;
        public Color color;
    }

    public List<ArrowData> activeArrows = new List<ArrowData>();
    public float shaftWidth = 10f;
    public float headSize = 30f;

    // Methode, um einen Pfeil zur Liste hinzuzuf�gen
    public void AddArrow(Vector2 from, Vector2 to, Color col)
    {
        activeArrows.Add(new ArrowData { start = from, end = to, color = col });
        SetVerticesDirty(); // Mesh neu zeichnen
    }

    public void ClearArrows()
    {
        activeArrows.Clear();
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        foreach (var arrow in activeArrows)
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
