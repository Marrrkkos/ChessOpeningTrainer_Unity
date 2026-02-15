using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json; // WICHTIG!

public static class TreeSerializer
{
    // Die "Magie-Einstellungen" für Newtonsoft
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        // 1. Das löst dein Problem mit Piece/Rook/Pawn automatisch!
        // Er schreibt "$type": "Rook" in die Datei.
        TypeNameHandling = TypeNameHandling.Auto, 

        // 2. Macht die Datei schön lesbar
        Formatting = Formatting.Indented, 

        // 3. Verhindert Abstürze bei Kreis-Referenzen (Safety First)
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
    };

    // --- SAVE ---
    public static void Save(Node root, string name, bool color, List<Move> moves, string filename)
    {
        TreeSaveData data = new TreeSaveData
        {
            treeName = name,
            useColor = color,
            openingMoves = moves,
            root = root // Einfach den Startknoten reinwerfen!
        };

        // EINE ZEILE macht die ganze Arbeit:
        string json = JsonConvert.SerializeObject(data, Settings);
        
        File.WriteAllText(Path.Combine(Application.persistentDataPath, filename), json);
        Debug.Log($"Gespeichert mit Newtonsoft: {filename}");
    }

    // --- LOAD ---
    public static Node Load(string filename, out string loadedName, out bool loadedColor, out List<Move> loadedMoves)
    {
        // Defaults
        loadedName = "";
        loadedColor = false;
        loadedMoves = new List<Move>();

        string path = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);

        // EINE ZEILE baut den ganzen Baum + Unterklassen wieder auf:
        TreeSaveData data = JsonConvert.DeserializeObject<TreeSaveData>(json, Settings);

        if (data == null) return null;

        // Daten zurückgeben
        loadedName = data.treeName;
        loadedColor = data.useColor;
        loadedMoves = data.openingMoves ?? new List<Move>();

        return data.root;
    }
}