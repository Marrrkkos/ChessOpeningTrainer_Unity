using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StockFishController : MonoBehaviour
{
    

    public Board board;
    public DrawOnBoard drawOnBoard;
    private Process engineProcess;
    private StreamWriter engineInput;
    
    public Text[] evals;
    public Text[] evalLines;

    // Wir nutzen ein Thread-sicheres Queue oder einen StringBuilder für die Antworten
    private StringBuilder outputBuffer = new StringBuilder();
    private readonly object lockObject = new object();
    private int currentArrowCount = 0;

    private Dictionary<int, string> currentTopMoves = new Dictionary<int, string>();
    public void DrawStockFishArrows(List<Move> gameMoves)
    {
        SendCommand("stop");
        currentTopMoves.Clear();
        drawOnBoard.arrow.ClearArrows(1);

        SendCommand("position startpos moves " + BoardUtil.GameToUCI(gameMoves, true));
        SendCommand("go infinite");
    }
    public void ChangeSettings()
    {
        SendCommand("setoption name MultiPV value " + GameManager.instance.settings.engineArrowCount);
    }
  
    void OnEnable()
    {
        UnityEngine.Debug.Log("Stockfish starting...");
        
        // Pfad anpassen (Linux-Binaries brauchen oft Ausführrechte!)
        string path = Path.Combine(Application.streamingAssetsPath, "stockfishes/stockfish_linux/stockfish");

        engineProcess = new Process();
        engineProcess.StartInfo.FileName = path;
        engineProcess.StartInfo.UseShellExecute = false;
        engineProcess.StartInfo.RedirectStandardInput = true;
        engineProcess.StartInfo.RedirectStandardOutput = true;
        engineProcess.StartInfo.CreateNoWindow = true;

        // Das ist der Trick: Event-basiertes Auslesen
        engineProcess.OutputDataReceived += (sender, e) => {
            if (!string.IsNullOrEmpty(e.Data))
            {
                lock (lockObject)
                {
                    outputBuffer.AppendLine(e.Data);
                }
            }
        };

        engineProcess.Start();
        engineProcess.BeginOutputReadLine(); // Startet das asynchrone Lesen
        engineInput = engineProcess.StandardInput;

        SendCommand("uci");
        SendCommand("isready");
    }

    void Update()
    {
        if(board.stockFishActive){
            
        if(currentArrowCount != GameManager.instance.settings.engineArrowCount)
        {
            currentArrowCount = GameManager.instance.settings.engineArrowCount;
            SendCommand("setoption name MultiPV value " + currentArrowCount);
        }
        // Hier holen wir die Daten sicher in den Main-Thread von Unity
        string currentOutput = "";
        lock (lockObject)
        {
            if (outputBuffer.Length > 0)
            {
                currentOutput = outputBuffer.ToString();
                outputBuffer.Clear();
            }
        }

        if (!string.IsNullOrEmpty(currentOutput))
        {
            bool arrowsNeedUpdate = false;
            string[] lines = currentOutput.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                int multiPvIndex = GetMultiPvRank(line);
                string firstMoveOfLine = GetFirstMoveFromInfo(line);
                

                if (multiPvIndex > 0 && !string.IsNullOrEmpty(firstMoveOfLine))
                {
                    currentTopMoves[multiPvIndex] = firstMoveOfLine;
                    arrowsNeedUpdate = true;
                }
            }
            if (arrowsNeedUpdate)
            {
                UpdateArrows();
                UpdateText(lines);
            }
        }
            
        
        }
    }
    private void UpdateText(string[] lines)
    {
        for(int i = 0; i < currentArrowCount; i++)
        {
            int pvIndex = lines[i].IndexOf(" pv ");
            string pvPart = lines[i].Substring(pvIndex + 4).Trim();
            evalLines[i].text = pvPart;
        }
    }
    private void UpdateArrows()
    {
        // Löscht alle alten Pfeile vom Typ 1
        drawOnBoard.arrow.ClearArrows(1);

        // Gehe durch alle gefundenen MultiPV-Ränge (1, 2, 3...)
        foreach (var kvp in currentTopMoves)
        {
            string move = kvp.Value;
            if (move.Length >= 4) // Sicherheitshalber überprüfen, z.B. "e2e4"
            {
                string from = move.Substring(0, 2);
                string to = move.Substring(2, 2);

                int fromIndex = BoardUtil.StringToIndex(from);
                int toIndex = BoardUtil.StringToIndex(to);

                // Hier zeichnen wir den Pfeil
                drawOnBoard.drawArrow(fromIndex, toIndex, 1);
            }
        }
    }

    private int GetMultiPvRank(string line)
    {
        // Sucht nach dem Wert direkt nach "multipv" in der Zeile
        string[] parts = line.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "multipv" && i + 1 < parts.Length)
            {
                if (int.TryParse(parts[i + 1], out int rank))
                {
                    return rank;
                }
            }
        }
        return 1; // Standard-Fallback
    }

    private string GetFirstMoveFromInfo(string line)
    {
        int pvIndex = line.IndexOf(" pv ");
        if (pvIndex != -1)
        {
            string pvPart = line.Substring(pvIndex + 4).Trim();
            string[] moves = pvPart.Split(' ');
            if (moves.Length > 0)
            {
                return moves[0]; // Nur den allerersten Zug dieser Variante nehmen!
            }
        }
        return null;
    }
    public void SendCommand(string command)
    {   
        if (engineInput != null)
        {
            engineInput.WriteLine(command);
        }
    }

    void OnApplicationQuit()
    {
        if (engineProcess != null && !engineProcess.HasExited)
        {
            engineProcess.Kill();
            engineProcess.Dispose();
        }
    }
    public void SetActive(bool active)
    {
        if (active)
        {
            DrawStockFishArrows(board.currentGame.playedMoves);
        }
        board.stockFishActive = active;
    }
}