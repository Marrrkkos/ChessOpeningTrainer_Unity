using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class OpeningTreesData
{
    public List<string> openingNames = new ();

    private static string PathToSaveFile => Path.Combine(Application.persistentDataPath, "openings_index.json");

    public void Save()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(PathToSaveFile, json);
        Debug.Log("Namensliste gespeichert!");
    }

    public static OpeningTreesData Load()
    {
        if (!File.Exists(PathToSaveFile))
        {
            return new OpeningTreesData();
        }

        string json = File.ReadAllText(PathToSaveFile);
        var data = JsonConvert.DeserializeObject<OpeningTreesData>(json);

        return data ?? new OpeningTreesData();
    }

}