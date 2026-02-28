using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(IsReference = true)]
[System.Serializable]
public class Node
{
    public Move move;
    public List<Node> children;
    public Node parentNode;
    public Node(Move move, Node parentNode) { 
        this.move = move;
        this.children = new List<Node>();
        this.parentNode = parentNode;
    }
    public Node()
    {
        this.children = new List<Node>();
    }
    public void addChild(Node node) {
        children.Add(node);
    }
}
