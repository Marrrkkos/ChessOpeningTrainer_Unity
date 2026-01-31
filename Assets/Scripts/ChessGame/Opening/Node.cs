using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Move move;
    public List<Node> children;

    public Node(Move move) { 
        this.move = move;
        this.children = new List<Node>();
    }

    public void addChild(Node node) {
        children.Add(node);
    }
}
