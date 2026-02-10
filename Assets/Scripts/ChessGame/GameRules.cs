using System.Collections.Generic;
using UnityEngine;

public class GameRules
{
    public static List<Vector2Int> removeChecks(int currentPos, List<Vector2Int> currentPossibles, Board board)
    {
        List<Vector2Int> finalPossibles = new List<Vector2Int>();
        Piece currentPiece = board.fields[currentPos].piece;

        if (currentPossibles.Count == null) {
            return finalPossibles;
        }
        //if (board.fields[currentPos].piece == null || board.whiteKing == null || board.blackKing == null)           // KRITISCHE STELLE
        //{
         //   return finalPossibles;
        //}
        Piece myKing = currentPiece.color ? board.whiteKing : board.blackKing;
        
        foreach (Vector2Int move in currentPossibles)
        {
            int targetIndex = move.x;
            int rule = move.y;

            currentPiece.doMove(targetIndex, rule, null, false);

            int kingPosToCheck = (currentPiece.id == 5) ? targetIndex : myKing.position;

            if (!checkOchecks(board, currentPiece.color))
            {
                finalPossibles.Add(move);
            }

            currentPiece.undoMove(false);
        }

        return finalPossibles;
    }
    static Vector2Int[] verticalDirections = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    static Vector2Int[] diagonalDirections = { new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };
    public static bool checkCheckMate(Board board, bool kingColorToCheck) {
        if (kingColorToCheck)
        {
            foreach (Piece piece in board.whitePieces)
            {
                int startPos = piece.position;
                List<Vector2Int> possibleMoves = piece.getPossibleMoves();
                List<Vector2Int> possibles = removeChecks(startPos, possibleMoves, board);

                if (possibles.Count != 0)
                {
                    return false;
                }

            }
        }
        else
        {
            foreach (Piece piece in board.blackPieces)
            {
                int startPos = piece.position;
                List<Vector2Int> possibleMoves = piece.getPossibleMoves();
                List<Vector2Int> possibles = removeChecks(startPos, possibleMoves, board);
                if (possibles.Count != 0)
                {
                    return false;
                }

            }
        }
        Debug.Log("Daluarak CheckMate");
        return true;
    }

    public static bool checkOchecks(Board board, bool kingColorToCheck)    // TRUE = SCHACH
    {
        int currentPos = kingColorToCheck ? board.whiteKing.position : board.blackKing.position;
        return checkOneDiagonals(board, currentPos, kingColorToCheck) ||
                checkLPositions(board, currentPos, kingColorToCheck) ||
                checkVerticalPositions(board, currentPos, kingColorToCheck) ||
                checkDiagonalPosition(board, currentPos, kingColorToCheck);
    }
    private static bool checkOneDiagonals(Board board, int currentPos, bool kingColorToCheck)
    {
        bool rotation = board.rotation;

        int direction = (kingColorToCheck == rotation) ? -1 : 1;
        int[] sideOffsets = { -1, 1 };

        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;

        foreach (int xOffset in sideOffsets)
        {
            int targetRow = currentRow + direction;
            int targetCol = currentCol + xOffset;

            if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
            {
                Piece p = board.fields[targetRow * 8 + targetCol].piece;

                if (p != null && p.color != kingColorToCheck && p.id == 0) return true;
            }
        }
        return false;
    }

    private static bool checkLPositions(Board board, int ownKingPos, bool kingColorToCheck)
    {
        List<int> possibleMoves = new List<int>();

        int currentRow = ownKingPos / 8;
        int currentCol = ownKingPos % 8;

        Vector2Int[] knightOffsets = {
            new(2, 1), new(2, -1),
            new(-2, 1), new(-2, -1),
            new(1, 2), new(1, -2),
            new(-1, 2), new(-1, -2)
        };

        foreach (Vector2Int offset in knightOffsets)
        {
            int checkRow = currentRow + offset.y;
            int checkCol = currentCol + offset.x;

            if (checkRow >= 0 && checkRow < 8 && checkCol >= 0 && checkCol < 8)
            {
                int targetIdx = checkRow * 8 + checkCol;
                Piece p = board.fields[targetIdx].piece;
                if (p != null)
                {
                    if (p.color != kingColorToCheck && p.id == 3)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private static bool checkDiagonalPosition(Board board, int currentPos, bool kingColorToCheck)
    {

        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;

        foreach (Vector2Int dir in diagonalDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                int checkRow = currentRow + (dir.y * i);
                int checkCol = currentCol + (dir.x * i);

                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                {
                    break;
                }
                int checkIdx = checkRow * 8 + checkCol;
                Piece p = board.fields[checkIdx].piece;

                if (p != null)
                {
                    if (p.color == kingColorToCheck)
                    {
                        break;
                    }

                    if (p.color != kingColorToCheck && (p.id == 1 || p.id == 4))
                    {
                        return true;
                    }
                    break;
                }

            }
        }

        return false;
    }

    private static bool checkVerticalPositions(Board board, int currentPos, bool kingColorToCheck)
    {

        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;

        foreach (Vector2Int dir in verticalDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                int checkRow = currentRow + (dir.y * i);
                int checkCol = currentCol + (dir.x * i);

                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                {
                    break;
                }


                int checkIdx = checkRow * 8 + checkCol;
                Piece p = board.fields[checkIdx].piece;

                if (p != null)
                {
                    if (p.color == kingColorToCheck)
                    {
                        break;
                    }


                    if (p.color != kingColorToCheck && (p.id == 2 || p.id == 4))
                    {
                        return true;
                    }
                    break;
                }

            }
        }

        return false;
    }
    public static List<Vector2Int> getDiagonal(Board board, int position)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        int currentRow = position / 8;
        int currentCol = position % 8;

        bool currentPieceColor = board.fields[position].piece.color;
        foreach (Vector2Int dir in diagonalDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                int checkRow = currentRow + (dir.y * i);
                int checkCol = currentCol + (dir.x * i);

                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                    break;

                int checkIdx = checkRow * 8 + checkCol;
                Piece p = board.fields[checkIdx].piece;

                if (p != null)
                {
                    if (p.color != currentPieceColor)
                    {
                        possibleMoves.Add(new Vector2Int(checkIdx, 0));
                    }
                    break;
                }

                possibleMoves.Add(new Vector2Int(checkIdx, 0));
            }
        }

        return possibleMoves;
    }
    public static List<Vector2Int> getVertical(Board board, int position)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        int currentPos = position;

        bool pieceColor = board.fields[currentPos].piece.color;

        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;



        foreach (Vector2Int dir in verticalDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                int checkRow = currentRow + (dir.y * i);
                int checkCol = currentCol + (dir.x * i);

                if (checkRow < 0 || checkRow >= 8 || checkCol < 0 || checkCol >= 8)
                    break;

                int checkIdx = checkRow * 8 + checkCol;
                Piece p = board.fields[checkIdx].piece;

                if (p != null)
                {
                    if (p.color != pieceColor)
                    {
                        possibleMoves.Add(new Vector2Int(checkIdx, 0));
                    }
                    break;
                }

                possibleMoves.Add(new Vector2Int(checkIdx, 0));
            }
        }

        return possibleMoves;
    }
}
