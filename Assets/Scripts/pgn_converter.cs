using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class pgn_converter : MonoBehaviour
{
    public Board board;

    public int count = 0;
    public List<Game> games = new List<Game>();

    Game game = new Game(new Player[] { new Player("", 0, true), new Player("", 0, false) });
    public void Start() {
        /*Thread normal = new Thread(() =>
        {
        string path = Path.Combine(Application.dataPath, "Ressources/Tests/smallTest.pgn");
            readPGN(path);
        });
        normal.Start();*/

        //BoardUtil.SANToMove(board, "Nf3", true, true);
    }
    public void readPGN(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            string line;

            





            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("White "))
                {
                    string name = line.Replace("[White \"", "");
                    name = name.Replace("\"]", "");
                    game.players[0].name = name;
                }
                if (line.StartsWith("WhiteElo "))
                {
                    string name = line.Replace("[WhiteElo \"", "");
                    name = name.Replace("\"]", "");
                    game.players[0].dwz = int.Parse(name);
                }
                if (line.StartsWith("Black "))
                {
                    string name = line.Replace("[Black \"", "");
                    name = name.Replace("\"]", "");
                    game.players[1].name = name;
                }
                if (line.StartsWith("BlackElo "))
                {
                    string name = line.Replace("[BlackElo \"", "");
                    name = name.Replace("\"]", "");
                    game.players[1].name = name;
                }

                // Ein PGN-Spiel endet meistens mit dem Resultat (1-0, 0-1 oder 1/2-1/2)
                if (line.EndsWith("1-0"))
                {
                    convertGame(line, 1);
                }
                else if (line.EndsWith("0-1"))
                {
                    convertGame(line, 2);
                }
                else if (line.EndsWith("1/2/1/2"))
                {
                    convertGame(line, 3);
                }
                else {
                    convertGame(line, 0);
                }
            }
        }
    }
    public void convertGame(string line, int result) {
        bool color = true;
        List<string> moves = ExtractMoves(line);
        foreach (string move in moves) {

            //Debug.Log(BoardUtil.SANToMove(board, move, color, true).from + " " + BoardUtil.SANToMove(board, move, color, true).to);
            //Game.SimpleMove sm = SAN_Handler.SANToMove(board, move, color);
            //game.loadedGame.Add(sm);
            //board.doSimpleMove(sm, false);
            color = !color;
            count++;
            Debug.Log(count);
        }
        game.result = result;
        GameManager.instance.gameDataBase.Add(game);
    }


    List<string> ExtractMoves(string rawPgn)
    {
        string noHeaders = Regex.Replace(rawPgn, @"\[.*?\]", "");
        string noComments = Regex.Replace(noHeaders, @"\{.*?\}", "");
        string noExplanationMarks = Regex.Replace(noComments, @"[!|?]", "");
        string oneLine = noExplanationMarks.Replace("\n", " ").Replace("\r", " ");
        string[] parts = oneLine.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        List<string> finalMoves = new List<string>();

        foreach (string part in parts)
        {
            if (part.Contains(".")) continue;

            if (part == "1-0" || part == "0-1" || part == "1/2-1/2" || part == "*") continue;

            finalMoves.Add(part);
        }

        return finalMoves;
    }
}
