using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class StockFishController : MonoBehaviour
{
    public DrawOnBoard drawOnBoard;
    private Process engineProcess;
    private StreamWriter engineInput;
    
    // Wir nutzen ein Thread-sicheres Queue oder einen StringBuilder für die Antworten
    private StringBuilder outputBuffer = new StringBuilder();
    private readonly object lockObject = new object();
    public void DrawStockFishArrows(List<Move> gameMoves)
    {
        SendCommand("stop");
        SendCommand("position startpos moves " + BoardUtil.GameToUCI(gameMoves, true));
        SendCommand("go infinite");
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
        if(GameManager.instance.stockFishActivated){
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
            string[] lines = currentOutput.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (line.StartsWith("info") && line.Contains(" pv "))
                {
                    string bestMove = GetMoveFromInfo(line);
                    if(!string.IsNullOrEmpty(bestMove)){
                        string from = bestMove.Substring(0, 2); // Start bei 0, Länge 2 -> "e2"
                        string to = bestMove.Substring(2, 2);   // Start bei 2, Länge 2 -> "e4"
                    
                        drawOnBoard.arrow.ClearArrows(1);
                        drawOnBoard.drawArrow(BoardUtil.StringToIndex(from), BoardUtil.StringToIndex(to), 1);
                    }
                }
            }
        }
        }
    }
private string GetMoveFromInfo(string line)
{
    // Wir suchen den Teil nach " pv "
    int pvIndex = line.IndexOf(" pv ");
    if (pvIndex != -1)
    {
        string pvPart = line.Substring(pvIndex + 4).Trim();
        return pvPart.Split(' ')[0]; // Nur den ersten Zug nehmen
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
}