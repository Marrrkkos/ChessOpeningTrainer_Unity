using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using Newtonsoft.Json;

[System.Serializable]
public class Opening
{
    // --- EINSTELLUNGEN FÜR NEWTONSOFT ---
    // Das sorgt dafür, dass Unterklassen (Polymorphie) und tiefe Bäume funktionieren
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
    
    [JsonProperty] public Node rootNode; 

    // --- KONSTRUKTOREN ---
    public Opening(string name, bool color, Texture2D startPos, List<Move> moves) 
    {
        this.color = color;
        this.name = name;
        this.rootNode = new Node();
        this.startPos = startPos;
        this.moves = new List<Move>(moves);
        Add(moves);
    }
    
    // Leerer Konstruktor für Newtonsoft (wichtig beim Laden!)
    public Opening() { }


    // --- SPEICHER LOGIK (DIREKT HIER) ---

    // Wir nutzen eine kleine Hilfsklasse NUR fürs Speichern,
    // damit wir genau kontrollieren, was in der Datei landet.
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
        // 1. Daten in den Container packen
        SaveDataContainer data = new SaveDataContainer
        {
            name = this.name,
            color = this.color,
            moves = this.moves,
            rootNode = this.rootNode
        };

        // 2. Pfad bauen
        string filename = "savegame" + openingName + ".json";
        string path = Path.Combine(Application.persistentDataPath, filename);

        // 3. Speichern (Newtonsoft macht den Rest)
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
            
            // Container laden
            SaveDataContainer data = JsonConvert.DeserializeObject<SaveDataContainer>(json, JsonSettings);

            if (data == null) return false;

            // Werte zurück in DIESE Instanz schreiben
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

    // --- DEINE RESTLICHE LOGIK (Unverändert) ---

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
    public int GetOpeningSize(){
    
    if (rootNode == null) 
    {
        return 0;
    }
    int count = 0;
    
    
    return FindCounterRecursive(rootNode, 0,new List<Move>(), count);
}

private int FindCounterRecursive(Node currentNode, int currentDepth, List<Move> currentPath, int count)
{
    bool isLeaf = currentNode.children == null || currentNode.children.Count == 0;

    if (isLeaf)
    {
        if (currentPath.Count > 0)
        {
            count++;
        }
        return count;
    }
    foreach (Node childNode in currentNode.children)
    {
        currentPath.Add(childNode.move);
        
        FindCounterRecursive(childNode, currentDepth + 1, currentPath, count);
        
        currentPath.RemoveAt(currentPath.Count - 1);
    }
    return count;
}
    public List<List<Move>> GetAllLines(int depth)
{
    List<List<Move>> allLines = new List<List<Move>>();
    
    if (rootNode == null || depth <= 0) 
    {
        return allLines;
    }
    
    FindLinesRecursive(rootNode, 0, depth, new List<Move>(), allLines);
    
    return allLines;
}

private void FindLinesRecursive(Node currentNode, int currentDepth, int maxDepth, List<Move> currentPath, List<List<Move>> allLines)
{
    bool isLeaf = currentNode.children == null || currentNode.children.Count == 0;

    if (currentDepth == maxDepth || isLeaf)
    {
        if (currentPath.Count > 0)
        {
            allLines.Add(new List<Move>(currentPath));
        }
        return;
    }
    foreach (Node childNode in currentNode.children)
    {
        currentPath.Add(childNode.move);
        
        FindLinesRecursive(childNode, currentDepth + 1, maxDepth, currentPath, allLines);
        
        currentPath.RemoveAt(currentPath.Count - 1);
    }
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
    public void PrintTreeDepth5()
    {
        Node node = rootNode;
        foreach(Node n in node.children)
        {
            Debug.Log(n.move.ToString());
            foreach(Node n2 in n.children)
            {
                Debug.Log(n2.move.ToString());
                foreach(Node n3 in n2.children)
                {
                    Debug.Log(n3.move.ToString());
                    foreach(Node n4 in n3.children)
                    {
                        Debug.Log(n4.move.ToString());
                        foreach(Node n5 in n4.children)
                        {   
                            Debug.Log(n5.move.ToString());
                        }
                    }
                }
            }
            Debug.Log("-------------------------------------------");
        }

    }
}
