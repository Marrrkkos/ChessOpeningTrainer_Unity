using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bishop : Piece
{
    override
    public char pieceChar
    { get; } = 'B';
    override
    public bool hasMoved
    { get; set; } = false;
    override
    public int id{ get; } = 1;
    override
    public bool color { get;  }
    override
    public int position { get; set; }
    public Sprite sprite;

    public Bishop(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        this.position = position;
    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        return GameRules.getDiagonal(board, position);
    }
    override
    public Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI) {
        return null;
    }

    override
    public Move undoSpecialMove(bool refreshGUI)
    {
        return null;
    }
}
