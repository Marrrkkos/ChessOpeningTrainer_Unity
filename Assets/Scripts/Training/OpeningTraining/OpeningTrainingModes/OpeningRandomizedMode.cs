using System.Collections.Generic;
using UnityEngine;

public class OpeningRandomizedMode : MonoBehaviour
{
    

    public void InitTraining(Opening opening, int depth)
    { 
    }

    public void ManageNext()
    {
        
    }
    public void ResetTraining(bool restart)
    {}
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Wähle einen zufälligen Index von 0 bis i
            int randomIndex = Random.Range(0, i + 1);

            // Tausche die Elemente
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}