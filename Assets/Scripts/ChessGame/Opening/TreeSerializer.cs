using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class TreeSerializer
{
    public static void Save(Node root, string name, bool color, List<Move> moves, string filename)
    {
        TreeSaveData saveData = new TreeSaveData();
        
        // A) Metadaten setzen
        saveData.treeName = name;
        saveData.useColor = color;
        saveData.openingMoves = moves;
        // B) Baum "flachklopfen" (Dein Algorithmus)
        List<Node> allRealNodes = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        
        if (root != null) queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            allRealNodes.Add(current);
            
            foreach (var child in current.children)
            {
                queue.Enqueue(child);
            }
        }

        foreach (Node realNode in allRealNodes)
        {
            FlatNode flatNode = new FlatNode();
            flatNode.move = realNode.move;

            foreach (Node child in realNode.children)
            {
                int index = allRealNodes.IndexOf(child);
                flatNode.childrenIndices.Add(index);
            }

            saveData.allNodes.Add(flatNode);
        }

        // D) Speichern
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, filename), json);
        Debug.Log("Gespeichert (Name: " + name + ", Color: " + color + ")");
    }

    // --- LOAD ---
    // WICHTIG: Wir nutzen 'out', um Name und Color zurückzugeben, 
    // da der Rückgabewert der Funktion schon für den 'Node' reserviert ist.
    public static Node Load(string filename, out string loadedName, out bool loadedColor, out List<Move> loadedMoves)
    {
        // Standardwerte, falls Laden fehlschlägt
        loadedName = "";
        loadedColor = false;
        loadedMoves = new List<Move>();

        string path = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        TreeSaveData saveData = JsonUtility.FromJson<TreeSaveData>(json);

        if (saveData == null || saveData.allNodes.Count == 0) return null;

        // A) Metadaten extrahieren
        loadedName = saveData.treeName;
        loadedColor = saveData.useColor;
        loadedMoves = saveData.openingMoves;
        
        // B) Baum rekonstruieren
        List<Node> reconstructedNodes = new List<Node>();
        foreach (var flatNode in saveData.allNodes)
        {
            Node n = new Node(flatNode.move);
            reconstructedNodes.Add(n);
        }

        for (int i = 0; i < saveData.allNodes.Count; i++)
        {
            FlatNode flat = saveData.allNodes[i];
            Node real = reconstructedNodes[i];

            foreach (int childIndex in flat.childrenIndices)
            {
                real.children.Add(reconstructedNodes[childIndex]);
            }
        }

        return reconstructedNodes[0];
    }
}