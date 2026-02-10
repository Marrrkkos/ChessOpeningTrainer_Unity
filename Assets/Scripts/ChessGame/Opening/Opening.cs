using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Opening
{
    public bool color;
    public string name;
    public Texture2D startPos;
    private Node rootNode;
    public Opening(string name, bool color, Texture2D startPos, List<Move> moves) {
        this.color = color;
        this.name = name;
        this.rootNode = new Node();
        this.startPos = startPos;

        add(moves);
    }



    /**
     * Adds the New Moves in moves to the Opening
     */
    public void add(List<Move> moves) {
        Node node = rootNode;
        bool newLine = true;

        

        foreach (Move move in moves) {
                
            // completly new line
            if (node.children.Count == 0)
            {
                node.addChild(new Node(move));
                node = node.children.First();
            }


            else {
                // go one move further (move exists already
                foreach (Node n in node.children) {

                    if (n.move.equals(move)) {
                        node = n;
                        newLine = false;
                    }

                    if (newLine) {
                        node.addChild(new Node(move));
                        node = node.children.Last();
                    }
                    
                    newLine = true;
                }
            }
        }
    }

    /**
     * removes everything after this moves
     */

    public void remove(List<Move> moves) {
        Node node = rootNode;

        // get last move in line
        foreach (Move m in moves) {
            foreach (Node n in node.children) { 
                if(n.move.equals(m)) {
                    node = n;
                }
            }
        }

        node.children = null;
    }

}
