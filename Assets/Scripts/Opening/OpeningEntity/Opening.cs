using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using Newtonsoft.Json;

[System.Serializable]
public class Opening
{
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public bool color;
    public string name;
    
    [JsonIgnore] public Texture2D startPos; 
    
    public List<Move> moves;

    [System.NonSerialized]
    [JsonProperty] public Node rootNode; 

    public Opening(string name, bool color, Texture2D startPos, List<Move> moves) 
    {
        this.color = color;
        this.name = name;
        this.rootNode = new Node();
        this.startPos = startPos;
        this.moves = new List<Move>(moves);
        Add(moves);
    }
    
    // empty constructor for newtonsoft
    public Opening() { }

    private class SaveDataContainer
    {
        public string name;
        public bool color;
        public List<Move> moves;
        public Node rootNode;
    }
    public void DeleteOpening(string openingName)
    {
        string filename = "savegame" + openingName + ".json";
        string path = Path.Combine(Application.persistentDataPath, filename);

         if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Datei erfolgreich gelöscht: " + path);
        }
        else
        {
            Debug.LogWarning("Löschen fehlgeschlagen: Datei nicht gefunden.");
        }
    }
    public void SaveGame(string openingName)
    {
        SaveDataContainer data = new SaveDataContainer
        {
            name = this.name,
            color = this.color,
            moves = this.moves,
            rootNode = this.rootNode
        };

        string filename = "savegame" + openingName + ".json";
        string path = Path.Combine(Application.persistentDataPath, filename);

        string json = JsonConvert.SerializeObject(data, JsonSettings);
        File.WriteAllText(path, json);

        Debug.Log($"Opening {openingName} gespeichert unter: {path}");
    }

    public bool LoadGame(string openingName)
    {
        string filename = "savegame" + openingName + ".json";
        string path = Path.Combine(Application.persistentDataPath, filename);

        if (!File.Exists(path)) return false;

        try 
        {
            string json = File.ReadAllText(path);
            
            SaveDataContainer data = JsonConvert.DeserializeObject<SaveDataContainer>(json, JsonSettings);

            if (data == null) return false;

            this.name = data.name;
            this.color = data.color;
            this.moves = data.moves ?? new List<Move>();
            this.rootNode = data.rootNode ?? new Node();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Fehler beim Laden von Opening {openingName}: {e.Message}");
            return false;
        }
    }


    public List<Move> GetMoves(List<Move> gameMoves)
    {
        if(gameMoves.Count == 0)
        {
            return GetMovesFromNode(rootNode.children);
        }

        Node node = rootNode;

        bool foundOne = false;
        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                if(n.move.Equals(m)) {
                    node = n;
                    foundOne = true;
                }
            }
            if(foundOne == false)
            {
                return new List<Move>();
            }
            foundOne = false;
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
    public List<Node> GetAllNodes()
    {
        List<Node> allNodes = new();
        if (rootNode == null) return allNodes;

        Queue<Node> queue = new();
        queue.Enqueue(rootNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current.move != null && current.move.movedPiece.color == this.color)
            {
                allNodes.Add(current);
            }

            if (current.children != null)
            {
                foreach (var child in current.children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        return allNodes;
    }
    public int GetNodeMovesSize(Node node)
{
    if (node == null) return 0;

    int count = 0;

    if (node.move != null && node.move.movedPiece.color == this.color)
    {
        count = 1;
    }

    if (node.children != null)
    {
        foreach (var child in node.children)
        {
            count += GetNodeMovesSize(child); 
        }
    }

    return count;
}
public int GetMaxDepth(Node node)
{
    if (node == null) return 0;

    if (node.children == null || node.children.Count == 0) return 1;

    int maxChildDepth = 0;

    foreach (var child in node.children)
    {
        int childDepth = GetMaxDepth(child);
        
        if (childDepth > maxChildDepth)
        {
            maxChildDepth = childDepth;
        }
    }

    return maxChildDepth + 1;
}
    public int GetNodeLeafSize(Node node){
    
    if (node == null) 
    {
        return 0;
    }
    
    return FindCounterRecursive(node, 0, new List<Move>());
}

private int FindCounterRecursive(Node currentNode, int currentDepth, List<Move> currentPath)
{
    bool isLeaf = currentNode.children == null || currentNode.children.Count == 0;

    if (isLeaf)
    {
        if (currentPath.Count > 0)
        {
            return 1;
        }
        return 0;
    }

    int totalCount = 0;

    foreach (Node childNode in currentNode.children)
    {
        currentPath.Add(childNode.move);
        
        totalCount += FindCounterRecursive(childNode, currentDepth + 1, currentPath);
        
        currentPath.RemoveAt(currentPath.Count - 1);
    }
    
    return totalCount;
}






    public void Add(List<Move> moves) {
        Node currentNode = rootNode;

        foreach (Move moveToProcess in moves) {
        
            Node foundChild = null;
        
            foreach (Node child in currentNode.children) {
                if (child.move.Equals(moveToProcess)) {
                   foundChild = child;
                   break;
                }
            }

            if (foundChild != null) {
                currentNode = foundChild;
            } 
            else {
                Node newNode = new Node(moveToProcess, currentNode);
                currentNode.addChild(newNode);
                currentNode = newNode;
            }
        }
        SaveGame(name);
    }
    public void Remove(List<Move> gameMoves, int nextMoveF1, int nextMoveF2) {
        Node node = rootNode;

        // get last move in line
        foreach (Move m in gameMoves) {
            foreach (Node n in node.children) { 
                if(n.move.Equals(m)) {
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
        SaveGame(name);
    }
    
}
