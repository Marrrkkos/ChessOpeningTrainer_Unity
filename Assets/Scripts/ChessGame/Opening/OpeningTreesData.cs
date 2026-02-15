using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class OpeningsTreesData
{
    public List<string> openingNames = new ();

    // Der Pfad zur Datei (immer gleich)
    private static string PathToSaveFile => Path.Combine(Application.persistentDataPath, "openings_index.json");

    // --- SPEICHERN (Instanz-Methode) ---
    public void Save()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(PathToSaveFile, json);
        Debug.Log("Namensliste gespeichert!");
    }

    // --- LADEN (Statische Methode) ---
    public static OpeningsTreesData Load()
    {
        if (!File.Exists(PathToSaveFile))
        {
            return new OpeningsTreesData(); // Gibt leere Liste zurück, wenn noch keine Datei da ist
        }

        string json = File.ReadAllText(PathToSaveFile);
        var data = JsonConvert.DeserializeObject<OpeningsTreesData>(json);

        return data ?? new OpeningsTreesData(); // Sicherheits-Check, falls Datei leer ist
    }

}