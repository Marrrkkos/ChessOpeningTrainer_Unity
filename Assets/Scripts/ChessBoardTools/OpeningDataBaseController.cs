using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

using System.Collections.Generic;
public class OpeningDataBaseController : MonoBehaviour
{
    public Board board;

    public List<OpeningLineController> openingLineControllers;
    private readonly string baseUrl = "https://explorer.lichess.ovh/masters";

    private readonly string normalFen = "rnbnkqrb/pppppppp/8/8/8/8/PPPPPPPP/RNBNKQRB w KQkq - 0 1";

    [System.Serializable]
    public class LichessOpeningData
    {   
        public int white;
        public int draws;
        public int black;
        public LichessMove[] moves;
    }

    [System.Serializable]
    public class LichessMove
    {
        public string uci;
        public string san;
        public int white;
        public int draws;
        public int black;
    }
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
                //Debug.Log("Daten empfangen: " + webRequest.downloadHandler.text);
                Parsetext(webRequest.downloadHandler.text);
                // Hier kannst du den JSON-String mit JsonUtility oder Newtonsoft.Json parsen
            }
            else
            {
                Debug.LogError("Fehler: " + webRequest.error);
            }
        }
    }

    public void Parsetext(string text)
    {
         // Wandelt den JSON-Text in unser C#-Objekt um
        LichessOpeningData data = JsonUtility.FromJson<LichessOpeningData>(text);

    // Sicherheitsüberprüfung, falls das JSON fehlerhaft/leer ist
    if (data == null || data.moves == null)
    {
        Debug.LogWarning("Fehler beim Parsen der Eröffnungsdaten oder keine Züge gefunden.");
        return;
    }
    for (int i = 0; i < data.moves.Length; i++)
    {

        if(i == 16){break;}


        LichessMove move = data.moves[i];
        float totalGames = move.white + move.draws + move.black;
        
        if (totalGames > 0)
        {
            // Berechne die prozentualen Raten
            float whiteWinRate = (move.white / totalGames) * 100f;
            float drawRate = (move.draws / totalGames) * 100f;
            float blackWinRate = (move.black / totalGames) * 100f;

            // Ausgabe in der Konsole
            //Debug.Log($"Zug: {data.moves[i].san} | Weiß gewinnt: {whiteWinRate:F1}% | Remis: {drawRate:F1}% | Schwarz gewinnt: {blackWinRate:F1}%");
            openingLineControllers[i].SetMove(move.uci, move.san, whiteWinRate, drawRate, blackWinRate);
        }
        else
        {
            Debug.Log($"Zug: {data.moves[i].san} hat keine verzeichneten Spiele.");
        }
    }
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            GetOpeningMoves(board.currentGame.playedMoves);
        }
        board.openingDataBaseActive = active;
    }
}
