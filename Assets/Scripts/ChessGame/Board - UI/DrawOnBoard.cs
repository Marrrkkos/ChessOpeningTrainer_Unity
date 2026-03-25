using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawOnBoard : MonoBehaviour
{
    public Board board;
    public UIArrow arrow;
    public BoardScaler boardScaler;

    private int lastMove1 = 0;   // FOR lastMoveIMages
    private int lastMove2 = 0;
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
        Color color = new Color();
        switch (index)
        {
            case 0: 
                color = UnityEngine.Color.lightGreen;
                break;
            case 1: 
                color = UnityEngine.Color.lightBlue;
                break;
            case 2:
                color = UnityEngine.Color.brown;
                break;
            default:
                color = UnityEngine.Color.white;
                break;
        }

        if(index == 1)
        {
            
        }

        arrow.AddArrow(BoardUtil.IndexToString(f1),BoardUtil.IndexToString(f2),pos1 * boardScaler.cellSize - offSet, pos2 * boardScaler.cellSize - offSet, color, index);
    }
    public void DrawLastMove()
    {   
        
        board.fields[lastMove1].lastMoveImage.gameObject.SetActive(false);
        board.fields[lastMove2].lastMoveImage.gameObject.SetActive(false);
        if(board.currentGame.playedMoves.Count != 0)
        {
            Move lastMove = board.currentGame.playedMoves.Last();
            board.fields[lastMove.from].lastMoveImage.gameObject.SetActive(true);
            board.fields[lastMove.to].lastMoveImage.gameObject.SetActive(true);
            lastMove1 = lastMove.from;
            lastMove2 = lastMove.to;
        }
    }
}
