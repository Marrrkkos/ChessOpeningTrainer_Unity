using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
public class SAN_Handler
{
    
    /*private static readonly Regex SanPattern = new Regex(@"^([NBRQK])?([a-h])?([1-8])?(x)?([a-h][1-8])(=[NBRQ])?([+#])?$");
    public static string MoveToSAN(Board board, Move move)
    {
        int specialRule = move.specialRule;
        if (specialRule == 1) return "O-O";
        if (specialRule == 2) return "O-O-O";

        string pieceChar = (move.movedPiece.pieceChar == 'P') ?  "" : move.movedPiece.pieceChar + "";
        string capture = (move.capturedPiece != null) ? "x" : "";
        string targetSquare = GetSquareString(BoardUtil.IndexToPos(move.to));
        string promotion = GetPromotionString(specialRule);
        string check = GetCheckCondition(move.check, move.checkMate);

        // Bei Bauernschl�gen (z.B. exd5) muss die Startlinie angegeben werden
        if (move.movedPiece.id == 0 && move.capturedPiece != null)
        {
            pieceChar = GetSquareString(BoardUtil.IndexToPos(move.from))[0].ToString();
        }

        string disambiguation = "";
        if (move.movedPiece.id != 0) // Nicht bei Bauern
        {
            disambiguation = GetDisambiguation(board, move);
        }

        return $"{pieceChar}{disambiguation}{capture}{targetSquare}{promotion}{check}";
    }

    public static Game.SimpleMove SANToMove(Board board, string san, bool isWhiteTurn, bool rotation)
    {
        int kingY = isWhiteTurn ? 7 : 0;

        if (san == "O-O" || san == "0-0")
        {
            int fromIndex = BoardUtil.PosToIndex(new Vector2Int(4, kingY));
            int toIndex = BoardUtil.PosToIndex(new Vector2Int(6, kingY));
            return new Game.SimpleMove { from = fromIndex, to = toIndex, specialRule = 1 };
        }
        if (san == "O-O-O" || san == "0-0-0")
        {
            // e1 -> c1
            int fromIndex = BoardUtil.PosToIndex(new Vector2Int(4, kingY));
            int toIndex = BoardUtil.PosToIndex(new Vector2Int(2, kingY));
            return new Game.SimpleMove { from = fromIndex, to = toIndex, specialRule = 2 };
        }

        Match match = SanPattern.Match(san);
        if (!match.Success)
        {
            Debug.LogError($"Ung�ltiges SAN Format: {san}");
            return default;
        }

        string pieceLetter = match.Groups[1].Value;
        string fromFile = match.Groups[2].Value;
        string fromRank = match.Groups[3].Value;
        string targetSqStr = match.Groups[5].Value;
        string promotionStr = match.Groups[6].Value;

        int targetIndex = ParseSquareToIndex(targetSqStr, rotation);
        int pieceID = string.IsNullOrEmpty(pieceLetter) ? 0 : GetPieceID(pieceLetter[0]);
        //Debug.Log("pieceID: " + pieceID + " fromFile: " + fromFile + " fromRank: " + fromRank + " targetIndex: " + targetIndex + " promotionStr: " + promotionStr);

        List<Piece> myPieces = isWhiteTurn ? board.whitePieces : board.blackPieces;
        List<Piece> candidates = new List<Piece>();

        foreach (Piece p in myPieces)
        {
            if (p.id != pieceID) continue;

            Vector2Int currentPos = BoardUtil.IndexToPos(p.position);

            if (!string.IsNullOrEmpty(fromFile) && GetFileChar(currentPos.x) != fromFile[0]) continue;
            if (!string.IsNullOrEmpty(fromRank) && GetRankChar(currentPos.y) != fromRank[0]) continue;

            var moves = GameRules.removeChecks(p.position, p.getPossibleMoves(), board);

            foreach (var m in moves)
            {
                if (m.x == targetIndex)
                {
                    candidates.Add(p);
                    break;
                }
            }
        }

        if (candidates.Count == 0)
        {
            Debug.Log("Keine Figur gefunden");
            return default;
        }

        Piece chosenPiece = candidates[0];

        if (candidates.Count > 1)
        {
            Debug.LogWarning($"Mehrdeutiger Zug '{san}'. W�hle {GetSquareString(BoardUtil.IndexToPos(chosenPiece.position))}.");
        }

        return new Game.SimpleMove
        {
            from = chosenPiece.position,
            to = targetIndex,
            specialRule = GetPromotionRuleID(promotionStr),
            san = san
        };
    }private static string GetDisambiguation(Board board, Move move)
    {
        List<Piece> allies = move.movedPiece.color ? board.whitePieces : board.blackPieces;
        List<Piece> competitors = new List<Piece>();

        Piece pieceSave = board.fields[move.to].piece;
        pieceSave.undoMove(false);
        foreach (Piece p in allies)
        {
            if (p == move.movedPiece) continue;
            if (p.id != move.movedPiece.id) continue;
            
            List<Vector2Int> possibles = GameRules.removeChecks(p.position, p.getPossibleMoves(), board);

            foreach (Vector2Int possible in possibles) {
                if(possible.x == move.to)
                {
                    competitors.Add(p);

                }
            }
        }
        pieceSave.doMove(move.to,move.specialRule, GameRules.removeChecks(pieceSave.position, pieceSave.getPossibleMoves(), board), false);

        if (competitors.Count == 0) return "";

        Vector2Int currentPos = BoardUtil.IndexToPos(move.from);
        bool fileConflict = false;
        bool rankConflict = false;

        foreach (var comp in competitors)
        {
            Vector2Int compPos = BoardUtil.IndexToPos(comp.position);
            if (compPos.x == currentPos.x) fileConflict = true;
            if (compPos.y == currentPos.y) rankConflict = true;
        }

        if (!fileConflict) return GetFileChar(currentPos.x).ToString();
        if (!rankConflict) return GetRankChar(currentPos.y).ToString();

        return GetSquareString(currentPos);
    }

    private static int GetPieceID(char c)
    {
        switch (char.ToUpper(c))
        {
            case 'B': return 1;
            case 'R': return 2;
            case 'N': return 3;
            case 'Q': return 4;
            case 'K': return 5;
            default: return 0;
        }
    }

    private static string GetPromotionString(int specialRule)
    {
        switch (specialRule)
        {
            case 5: return "=Q";
            case 6: return "=N";
            case 7: return "=R";
            case 8: return "=B";
            default: return "";
        }
    }

    private static int GetPromotionRuleID(string promoStr)
    {
        if (string.IsNullOrEmpty(promoStr)) return 0;
        if (promoStr == "=Q") return 5;
        if (promoStr == "=N") return 6;
        if (promoStr == "=R") return 7;
        if (promoStr == "=B") return 8;
        return 0;
    }

    private static string GetCheckCondition(bool check, bool checkMate)
    {
        if (checkMate) return "#";
        if (check) return "+";
        return "";
    }

    private static int ParseSquareToIndex(string square)
    {
        if (string.IsNullOrEmpty(square) || square.Length < 2) return 0;
        int file = square[0] - 'a';
        int rank = square[1] - '1';

        return (7 - file) * 8 + rank;       // CHANGED!
    }
    private static string GetSquareString(Vector2Int pos)
    {
        return $"{GetFileChar(pos.x)}{GetRankChar(pos.y)}";
    }

    private static char GetFileChar(int x) => (char)('a' + x);
    private static char GetRankChar(int y) => (char)('8' - y);*/
}