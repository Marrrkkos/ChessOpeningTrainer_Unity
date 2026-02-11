using System;
using System.Collections.Generic;
using System.Linq;
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
}