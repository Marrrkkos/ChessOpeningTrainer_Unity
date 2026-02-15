using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnBoard : MonoBehaviour
{
    public Board board;
    public UIArrow arrow;
    public BoardScaler boardScaler;

    private List<int> currentPossibles = new List<int>();
    public void drawPossibles(List<Vector2Int> possibleMoves, bool refreshGUI) {
        foreach (Vector2Int pm in possibleMoves) {
            int id = pm.x;
            if (refreshGUI)
            {
                board.fields[id].possibleImage.gameObject.SetActive(true);
            }
            currentPossibles.Add(id);
        }
    }
    public void refreshPossibles(bool refreshGUI) {
        if (refreshGUI)
        {
            foreach (int i in currentPossibles) {
            
            board.fields[i].possibleImage.gameObject.SetActive(false);
                }
        }
        currentPossibles.Clear();
    }
    public void drawArrow(int f1, int f2, int index) {
        Vector2 pos1 = new Vector2(f1 % 8, -f1 / 8);
        Vector2 pos2 = new Vector2(f2 % 8, -f2 / 8);
        float offSetF = 3.5f*boardScaler.cellSize;
        Vector2 offSet = new Vector2(offSetF, -offSetF);
        Debug.Log("BoardScaler: " + boardScaler.cellSize + " " + pos1 + " " + pos2);
        arrow.AddArrow(BoardUtil.IndexToString(f1),BoardUtil.IndexToString(f2),pos1 * boardScaler.cellSize - offSet, pos2 * boardScaler.cellSize - offSet, UnityEngine.Color.lightGreen, index);
    }
    public void drawSelectedField(string f) { }
}
