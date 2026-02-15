using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeSaveData
{
    public string treeName = "";
    public bool useColor = true;
    public List<Move> openingMoves = new List<Move>();
    public Node root;
}