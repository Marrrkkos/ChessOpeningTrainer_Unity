using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Rook : Piece
{
    override
    public char pieceChar
    { get; } = 'R';
    override
    public bool hasMoved { get; set; } = false;
    override
    public int id{ get; } = 2;
    override
    public bool color { get; }
    override
    public int position { get; set; }
    public Sprite sprite;

    public Rook(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        hasMoved = false;
        this.position = position;
    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        return GameRules.getVertical(board, position);
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
