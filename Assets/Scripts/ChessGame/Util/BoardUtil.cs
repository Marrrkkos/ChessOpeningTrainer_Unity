using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoardUtil
{

    public static Vector2Int IndexToPos(int index) => new Vector2Int(index % 8, index / 8);
    public static int PosToIndex(Vector2Int pos) => pos.y * 8 + pos.x;

    public static bool IsOnBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }
    public static int StringToIndex(string fieldName)
    {
        int letter = fieldName[0] - 'a';
        int number = fieldName[1] - '1';

        return (7 - number) * 8 + letter;
    }
    public static string IndexToString(int id)
    {
        int letterIndex;
        int numberIndex;

        // Umkehrung von: (7 - number) * 8 + letter
        letterIndex = id % 8;
        numberIndex = 7 - (id / 8);

        // Umwandlung der Zahlen (0-7) zur�ck in Zeichen ('a'-'h' und '1'-'8')
        char letter = (char)('a' + letterIndex);
        char number = (char)('1' + numberIndex);

        return $"{letter}{number}";
        
    }
    public static string GetSquareString(Vector2Int pos)
    {
        return $"{GetFileChar(pos.x)}{GetRankChar(pos.y)}";
    }

    public static char GetFileChar(int x) => (char)('a' + x);
    public static char GetRankChar(int y) => (char)('8' - y);

    public static string GameToUCI(List<Move> gameMoves, bool stockfish)
    {
    StringBuilder sb = new StringBuilder("");
    for (int i = 0; i < gameMoves.Count; i++)
    {
        if(i != 0)
        {
            if(stockfish){sb.Append(" ");}else{sb.Append(",");}
        }

        sb.Append(BoardUtil.IndexToString(gameMoves[i].from)).Append(BoardUtil.IndexToString(gameMoves[i].to));
        if (gameMoves[i].specialRule >= 5)
        {
            sb.Append(BoardUtil.GetPromotionString(gameMoves[i].specialRule, false));
        }
    }
    return sb.ToString();
    }
    public static char IdToChar(int id, bool upper)
    {
        char c = ' ';
        switch (id)
        {
            case 0: c = 'p'; break;
            case 1: c = 'b'; break;
            case 2: c = 'r'; break;
            case 3: c = 'n'; break;
            case 4: c = 'q'; break;
            case 5: c = 'k'; break;
            default: return '?';
        }

        return upper ? char.ToUpper(c) : c;
    }

    public static int CharToInt(char pieceChar)
    {
        switch (char.ToLower(pieceChar))
        {
            case 'p': return 0;
            case 'b': return 1;
            case 'r': return 2;
            case 'n': return 3;
            case 'q': return 4;
            case 'k': return 5;
            default: return -1;
        }
    }
    public static string GetPromotionString(int specialRule, bool san)
    {
        if(san){
            switch (specialRule)
            {
                case 5: return "=Q";
                case 6: return "=N";
                case 7: return "=R";
                case 8: return "=B";
                default: return "";
            }
        }
        else
        {
            switch (specialRule)
            {
                case 5: return "q";
                case 6: return "n";
                case 7: return "r";
                case 8: return "b";
                default: return "";
            }
        }
    }
}