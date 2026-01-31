using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Queen : Piece
{
    override
    public char pieceChar
    { get; } = 'Q';
    override
    public bool hasMoved
    { get; set; } = false;
    override
    public int id{ get; } = 4;
    override
    public bool color { get; }
    override
    public int position { get; set; }
    public Sprite sprite;
    public Queen(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        this.position = position;
    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        List<Vector2Int> l = GameRules.getVertical(board, position);
        List<Vector2Int> l2 = GameRules.getDiagonal(board, position);
        l.AddRange(l2);
        return l;
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
