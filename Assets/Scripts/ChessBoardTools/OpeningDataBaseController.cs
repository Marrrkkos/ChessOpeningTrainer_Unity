using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System; // Für Exception Handling beim Parsen

public class OpeningDataBaseController : MonoBehaviour
{
    public Board board;
    public List<OpeningLineController> openingLineControllers;
    private readonly string baseUrl = "https://explorer.lichess.ovh/masters";


    private int maxRetries = 3;
    private int currentRetryCount = 0; 
    private float retryDelay = 3.0f;


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
    
        currentRetryCount = 0; 
        
        StopAllCoroutines(); 
        StartCoroutine(FetchOpeningData(gameMoves));
    }

    IEnumerator FetchOpeningData(List<Move> gameMoves)
    {
        string url = $"{baseUrl}?play={BoardUtil.GameToUCI(gameMoves, false)}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 5;
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogWarning($"Netzwerkfehler: {webRequest.error}");
                    HandleNetworkError(gameMoves);
                    break;

                case UnityWebRequest.Result.Success:
                    currentRetryCount = 0; 
                    ParseText(webRequest.downloadHandler.text, gameMoves);
                    break;
            }
        }
    }

    private void HandleNetworkError(List<Move> gameMoves)
    {
        ClearOpeningLines();

        if (currentRetryCount < maxRetries)
        {
            currentRetryCount++;
            StartCoroutine(RetryFetch(gameMoves));
        }
        else
        {
            return;
        }
    }

    IEnumerator RetryFetch(List<Move> gameMoves)
    {
        yield return new WaitForSeconds(retryDelay);
        StartCoroutine(FetchOpeningData(gameMoves));
    }

    public void ParseText(string text, List<Move> gameMoves)
    {
        try 
        {
            LichessOpeningData data = JsonUtility.FromJson<LichessOpeningData>(text);

            if (data == null || data.moves == null)
            {
                HandleNetworkError(gameMoves);
                return;
            }

            ClearOpeningLines();

            for (int i = 0; i < data.moves.Length; i++)
            {
                if (i >= openingLineControllers.Count) break; 

                LichessMove move = data.moves[i];
                float totalGames = move.white + move.draws + move.black;
                
                if (totalGames > 0)
                {
                    float whiteWinRate = (move.white / totalGames) * 100f;
                    float drawRate = (move.draws / totalGames) * 100f;
                    float blackWinRate = (move.black / totalGames) * 100f;

                    openingLineControllers[i].gameObject.SetActive(true);
                    openingLineControllers[i].SetMove(move.uci, move.san, whiteWinRate, drawRate, blackWinRate);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Parse-Exception: {e.Message}");
            HandleNetworkError(gameMoves);
        }
    }

    private void ClearOpeningLines()
    {
        foreach (var controller in openingLineControllers)
        {
            controller.gameObject.SetActive(false); 
        }
    }

    public void SetActive(bool active)
    {
        board.openingDataBaseActive = active;
        if (active)
        {
            GetOpeningMoves(board.currentGame.playedMoves);
        }
        else 
        {
            ClearOpeningLines();
        }
    }
}