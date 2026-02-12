using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

[System.Serializable]
public class Opening
{
    public bool color;
    public string name;
    public Texture2D startPos;
    public List<Move> moves;
    private Node rootNode;
    public Opening(string name, bool color, Texture2D startPos, List<Move> moves) {
        this.color = color;
        this.name = name;
        this.rootNode = new Node();
        this.startPos = startPos;
        this.moves = new List<Move>(moves);

        add(moves);
    }

    public List<Move> GetMoves(List<Move> gameMoves)
    {
        Node node = rootNode;

        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                if(n.move.equals(m)) {
                    node = n;
                }
            }
        }

        return GetMovesFromNode(node.children);
    }
    private List<Move> GetMovesFromNode(List<Node> children)
    {
        List<Move> moves = new List<Move>();

        foreach(Node n in children)
        {
            moves.Add(n.move);
        }
        return moves;

    }
    /**
     * Adds the New Moves in moves to the Opening
     */
    public void add(List<Move> moves) {
    Node currentNode = rootNode;

    foreach (Move moveToProcess in moves) {
        
        Node foundChild = null;
        
        foreach (Node child in currentNode.children) {
            if (child.move.equals(moveToProcess)) {
                foundChild = child;
                break;
            }
        }

        if (foundChild != null) {
            currentNode = foundChild;
        } 
        else {
            Node newNode = new Node(moveToProcess);
            currentNode.addChild(newNode);
            currentNode = newNode;
        }
    }
}
    /**
     * removes everything after this moves
     */

    public void remove(List<Move> gameMoves) {
        Node node = rootNode;

        Move lastMove = gameMoves.Last();
        gameMoves.Remove(lastMove);
        // get last move in line
        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                if(n.move.equals(m)) {
                    node = n;
                }
            }
        }

        foreach (Node n in node.children) { 
            if(n.move.equals(lastMove)) {
                node.children.Remove(n);
            }
        }
    }

}
