using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class StockFishController : MonoBehaviour
{


    public Board board;
    public DrawOnBoard drawOnBoard;
    private Process engineProcess;
    private StreamWriter engineInput;
    public Text[] evalLines;
    public SliderControllerFillStand engineBarSliderController;
    // Wir nutzen ein Thread-sicheres Queue oder einen StringBuilder für die Antworten
    private StringBuilder outputBuffer = new StringBuilder();
    private readonly object lockObject = new object();
    private int currentArrowCount = 0;

    private Dictionary<int, string> currentTopMoves = new Dictionary<int, string>();
    private Dictionary<int, string> currentPvTexts = new Dictionary<int, string>();
    private Dictionary<int, string> currentEvals = new Dictionary<int, string>(); // NEU
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
        currentArrowCount = GameManager.instance.settings.engineArrowCount;

        SendCommand("stop");

        SendCommand("setoption name MultiPV value " + currentArrowCount);

        if (board.gameController.stockFishActive)
        {
            DrawStockFishArrows(board.currentGame.playedMoves);
        }
    }

    void OnEnable()
    {
        engineBarSliderController.SetMinMax(-500, 500);

        string path = Path.Combine(Application.streamingAssetsPath, "stockfishes/stockfish_windows/stockfish/stockfish-windows-x86-64-avx2.exe");

        engineProcess = new Process();
        engineProcess.StartInfo.FileName = path;
        engineProcess.StartInfo.UseShellExecute = false;
        engineProcess.StartInfo.RedirectStandardInput = true;
        engineProcess.StartInfo.RedirectStandardOutput = true;
        engineProcess.StartInfo.CreateNoWindow = true;

        // Das ist der Trick: Event-basiertes Auslesen
        engineProcess.OutputDataReceived += (sender, e) =>
        {
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
        if (board.gameController.stockFishActive)
        {

            if (currentArrowCount != GameManager.instance.settings.engineArrowCount)
            {
                ChangeSettings();
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
                bool needsUpdate = false;
                string[] lines = currentOutput.Split('\n');

                foreach (string line in lines)
                {
                    // Only process valid UCI info lines that contain principal variations
                    if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("info") || !line.Contains(" pv "))
                        continue;

                    int multiPvIndex = GetMultiPvRank(line);
                    string firstMoveOfLine = GetFirstMoveFromInfo(line);
                    string score = GetScoreFromInfo(line); // NEU: Score auslesen

                    if (multiPvIndex > 0 && !string.IsNullOrEmpty(firstMoveOfLine))
                    {
                        currentTopMoves[multiPvIndex] = firstMoveOfLine;
                        currentEvals[multiPvIndex] = score; // NEU: Score speichern

                        int pvIndex = line.IndexOf(" pv ");
                        currentPvTexts[multiPvIndex] = line.Substring(pvIndex + 4).Trim();

                        needsUpdate = true;
                    }
                }

                if (needsUpdate)
                {
                    UpdateArrows();
                    UpdateText(); // Use the new method
                    UpdateEngineBar();
                }
            }


        }
    }
    private void UpdateEngineBar()
{
    if (currentEvals.ContainsKey(1))
    {
        string evalString = currentEvals[1];
        long fillValue = 0;

        if (evalString.Contains("M"))
        {
            fillValue = evalString.StartsWith("-") ? -1000 : 1000;
        }
        else
        {
            string cleanEval = evalString.Replace("+", "").Replace(",", ".");

            if (float.TryParse(cleanEval, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatEval))
            {
                fillValue = (long)(floatEval * 100);
            }
        }

        engineBarSliderController.SetFillStand(fillValue);
    }
}
    private void UpdateText()
    {
        int maxLines = Mathf.Min(currentArrowCount, evalLines.Length);

        for (int i = 0; i < maxLines; i++)
        {
            int rank = i + 1;
            if (currentPvTexts.ContainsKey(rank) && currentEvals.ContainsKey(rank))
            {
                // combine eval and moves
                evalLines[i].text = $"[{currentEvals[rank]}] {currentPvTexts[rank]}";
            }
        }
    }
    private void UpdateArrows()
    {
        drawOnBoard.arrow.ClearArrows(1);

        foreach (var kvp in currentTopMoves)
        {
            string move = kvp.Value;
            if (move.Length >= 4)
            {
                string from = move.Substring(0, 2);
                string to = move.Substring(2, 2);

                int fromIndex = BoardUtil.StringToIndex(from);
                int toIndex = BoardUtil.StringToIndex(to);

                drawOnBoard.drawArrow(fromIndex, toIndex, 1);
            }
        }
    }
    private string GetScoreFromInfo(string line)
    {
        string[] parts = line.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "score" && i + 2 < parts.Length)
            {
                string type = parts[i + 1];
                string valStr = parts[i + 2];

                if (type == "cp" && int.TryParse(valStr, out int cp))
                {
                    float eval = cp / 100f;
                    return (eval > 0 ? "+" : "") + eval.ToString("0.00");
                }
                else if (type == "mate" && int.TryParse(valStr, out int mate))
                {
                    return "M" + mate;
                }
            }
        }
        return "0.00";
    }
    private int GetMultiPvRank(string line)
    {
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
        return 1;
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
                return moves[0];
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
        board.gameController.stockFishActive = active;
    }
}