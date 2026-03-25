using UnityEngine;
using System.Text;
using System;

public class FEN_Handler
{
    public struct FenData
    {
        public int[] pieceIds;
        public int[] pieceColors;
        public bool isWhiteTurn;
        public string castlingRights;
        public string enPassant;
        public int halfMoveClock;
        public int fullMoveNumber;
    }
    public static string GetStartPositionFen()
    {
        return "rnbnkqrb/pppppppp/8/8/8/8/PPPPPPPP/RNBNKQRB w KQkq - 0 1";
    }
    public const int EMPTY_SQUARE_ID = -1;
    public const int WHITE = 0;
    public const int BLACK = 1;

    public static string BoardToFEN(Board board)
    {
        StringBuilder fen = new StringBuilder();

        for (int i = 0; i < 8; i++)
        {
            int emptyCount = 0;

            for (int j = 0; j < 8; j++)
            {
                int fieldIndex = i * 8 + j;
                if(fieldIndex >= board.fields.Length) break;

                var field = board.fields[fieldIndex];

                if (field.piece != null)
                {
                    if (emptyCount > 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    bool isWhite = field.piece.color; 
                    char pieceChar = BoardUtil.IdToChar(field.piece.id, isWhite);
                    
                    fen.Append(pieceChar);
                }
                else
                {
                    emptyCount++;
                }
            }

            if (emptyCount > 0)
            {
                fen.Append(emptyCount);
            }

            if (i != 7)
            {
                fen.Append('/');
            }
        }

        string color = (board.currentGame.currentPlayer == WHITE) ? "w" : "b";
        fen.Append(" " + color);

        string castling = GetCastlingString(board); 
        fen.Append(" " + castling);

        string enPassant = GetEnPassantString(board);
        fen.Append(" " + enPassant);

        fen.Append(" 0"); 

        fen.Append(" 1");

        return fen.ToString();
    }

    public static FenData FENToPos(string fen)
    {
        FenData data = new FenData();
        data.pieceIds = new int[64];
        data.pieceColors = new int[64];
        
        for(int k = 0; k < 64; k++) 
        {
            data.pieceIds[k] = EMPTY_SQUARE_ID;
            data.pieceColors[k] = -1; 
        }

        string[] sections = fen.Split(' ');

        string placement = sections[0];
        int squareIndex = 0;

        for (int i = 0; i < placement.Length; i++)
        {
            char c = placement[i];

            if (c == '/') continue;

            if (char.IsDigit(c))
            {
                int emptySquares = c - '0';
                squareIndex += emptySquares;
            }
            else
            {
                int pieceID = BoardUtil.CharToInt(c);
                int pieceColor = char.IsUpper(c) ? WHITE : BLACK;
                
                if (squareIndex < 64)
                {
                    data.pieceIds[squareIndex] = pieceID;
                    data.pieceColors[squareIndex] = pieceColor;
                    squareIndex++;
                }
            }
        }

        if (sections.Length > 1)
        {
            data.isWhiteTurn = (sections[1] == "w");
        }
        else
        {
            data.isWhiteTurn = true;
        }

        if (sections.Length > 2)
        {
            data.castlingRights = sections[2];
        }
        else
        {
            data.castlingRights = "-";
        }

        if (sections.Length > 3)
        {
            data.enPassant = sections[3];
        }
        else
        {
            data.enPassant = "-";
        }

        return data;
    }


    private static string GetCastlingString(Board board)
    {
        //TODO
        return "KQkq";
    }

    private static string GetEnPassantString(Board board)
    {
        // TODO
        
        return "-"; 
    }
}