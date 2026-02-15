using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlatNode
{
    // Deine echten Daten
    public Move move;
    
    // Statt Node-Referenzen speichern wir hier nur Indices (Zahlen)
    // "Mein erstes Kind ist an Stelle 5 in der Liste"
    public List<int> childrenIndices = new List<int>();
}