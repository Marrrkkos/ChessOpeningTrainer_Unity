using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

using System.Collections.Generic;
public class OpeningDataBaseController : MonoBehaviour
{
    
    private readonly string baseUrl = "https://explorer.lichess.ovh/masters";

    private readonly string normalFen = "rnbnkqrb/pppppppp/8/8/8/8/PPPPPPPP/RNBNKQRB w KQkq - 0 1";

    public void GetOpeningMoves(List<Move> gameMoves)
    {
        StartCoroutine(FetchOpeningData(gameMoves));
    }

    IEnumerator FetchOpeningData(List<Move> gameMoves)
    {
        // FEN muss für die URL "escaped" werden
        string url = $"{baseUrl}?play={BoardUtil.GameToUCI(gameMoves, false)}";
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Daten empfangen: " + webRequest.downloadHandler.text);
                // Hier kannst du den JSON-String mit JsonUtility oder Newtonsoft.Json parsen
            }
            else
            {
                Debug.LogError("Fehler: " + webRequest.error);
            }
        }
    }
    public void DrawOpeningDataBaseArrows(List<Move> gameMoves)
    {
        GetOpeningMoves(gameMoves);
    }
}
