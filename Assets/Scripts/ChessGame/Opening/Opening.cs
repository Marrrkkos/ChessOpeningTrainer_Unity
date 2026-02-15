using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

[System.Serializable]
public class Opening
{
    public int openingID;
    public bool color;
    public string name;
    public Texture2D startPos;
    public List<Move> moves;
    private Node rootNode;
    public Opening(string name, bool color, Texture2D startPos, List<Move> moves, int openingID) {
        this.color = color;
        this.name = name;
        this.rootNode = new Node();
        this.startPos = startPos;
        this.moves = new List<Move>(moves);
        this.openingID = openingID;
        add(moves);
    }
    public Opening(int openingID)
    {
        this.openingID = openingID;
    }
    public List<Move> GetMoves(List<Move> gameMoves)
    {
        if(gameMoves.Count == 0)
        {
            return GetMovesFromNode(rootNode.children);
        }

        Node node = rootNode;

        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                Debug.Log(n.move.ToString() + "  ||  " + m.ToString());
                if(n.move.equals(m)) {
                    node = n;
                }
            }
        }
        if(node == rootNode)
        {
            return new List<Move>();
        }

        return GetMovesFromNode(node.children);
    }
    private List<Move> GetMovesFromNode(List<Node> children)
    {
        List<Move> movesSave = new List<Move>();

        foreach(Node n in children)
        {
            movesSave.Add(n.move);
        }
        return movesSave;

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
    SaveGame(openingID);
}
    /**
     * removes everything after this moves
     */

    public void remove(List<Move> gameMoves, int nextMoveF1, int nextMoveF2) {
        Node node = rootNode;

        // get last move in line
        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                if(n.move.equals(m)) {
                    node = n;
                }
            }
        }

        
        foreach (Node n in node.children) { 
            if(n.move.from == nextMoveF1 && n.move.to == nextMoveF2) {
                node.children.Remove(n);
                return;
            }
        }
        SaveGame(openingID);
    }

    public void SaveGame(int i)
    {
        TreeSerializer.Save(rootNode,name,color,moves, "savegame" + i + ".json");
    }

    public bool LoadGame(int i)
    {
        
        Node loadedNode = TreeSerializer.Load("savegame" + i + ".json", out name, out color, out moves);

        if(loadedNode != null)
        {
            rootNode = loadedNode;
            return true;
        }
        return false;
    }

    public void PrintTreeDepth5()
    {
        Node node = rootNode;
        foreach(Node n in node.children)
        {
            Debug.Log(BoardUtil.IndexToString(n.move.from )+ " " + BoardUtil.IndexToString(n.move.to));
            foreach(Node n2 in n.children)
            {
                Debug.Log(BoardUtil.IndexToString(n2.move.from )+ " " + BoardUtil.IndexToString(n2.move.to));
                foreach(Node n3 in n2.children)
                {
                    Debug.Log(BoardUtil.IndexToString(n3.move.from )+ " " + BoardUtil.IndexToString(n3.move.to));
                    foreach(Node n4 in n3.children)
                    {
                        Debug.Log(BoardUtil.IndexToString(n4.move.from )+ " " + BoardUtil.IndexToString(n4.move.to));
                        foreach(Node n5 in n4.children)
                        {   
                            Debug.Log(BoardUtil.IndexToString(n5.move.from )+ " " + BoardUtil.IndexToString(n5.move.to));
                        }
                    }
                }
            }
            Debug.Log("-------------------------------------------");
        }

    }
}
