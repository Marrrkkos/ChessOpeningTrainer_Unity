using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Knight : Piece
{
    override
    public char pieceChar
    { get; } = 'N';
    override
    public bool hasMoved { get; set; } = false;
    override
    public int id{ get; } = 3;
    override
    public bool color { get; }
    override
    public int position { get; set; }
    public Sprite sprite;
    public Knight(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        this.position = position;
    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        int currentPos = this.position;
        
        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;

        Vector2Int[] knightOffsets = {
        new(2, 1), new(2, -1),
        new(-2, 1), new(-2, -1),
        new(1, 2), new(1, -2),
        new(-1, 2), new(-1, -2)
    };

        foreach (Vector2Int offset in knightOffsets)
        {
            int targetRow = currentRow + offset.y;
            int targetCol = currentCol + offset.x;

            if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
            {
                int targetIdx = targetRow * 8 + targetCol;
                Piece p = this.board.fields[targetIdx].piece;

                if (p == null || p.color != this.color)
                {
                    possibleMoves.Add(new Vector2Int(targetIdx, 0));
                }
            }
        }
        return possibleMoves;
    }
    override
    public Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI)
    {
        return null;
    }

    override
    public Move undoSpecialMove(bool refreshGUI)
    {
        return null;
    }
}

