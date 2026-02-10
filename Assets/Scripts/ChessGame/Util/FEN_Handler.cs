using UnityEngine;
using System.Text;
public class FEN_Handler
{
    public const int EMPTY_SQUARE_ID = -1;
    public static string BoardToFEN(Board board)
    {
        StringBuilder fen = new StringBuilder();
        int emptyCount = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int fieldIndex = i * 8 + j;
                var field = board.fields[fieldIndex];

                if (field.piece != null)
                {
                    if (emptyCount > 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    char pieceChar = BoardUtil.IdToChar(field.piece.id, field.piece.color);
                    
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
                emptyCount = 0;
            }

            if (i != 7)
            {
                fen.Append('/');
            }
        }

        string color = (board.currentGame.currentPlayer == 0) ? "w" : "b";
        fen.Append(" " + color);

        fen.Append(" KQkq"); 

        fen.Append(" -");

        fen.Append(" 0");

        fen.Append(" 1");

        return fen.ToString();
    }

    public static int[] FENToPos(string fen)
    {
        int[] pieces = new int[64];
        
        for(int k = 0; k < 64; k++) pieces[k] = EMPTY_SQUARE_ID;

        int squareIndex = 0;

        for (int i = 0; i < fen.Length; i++)
        {
            char c = fen[i];

            if (c == ' ') break;

            if (c == '/') continue;

            if (char.IsDigit(c))
            {
                int emptySquares = c - '0';
                
                squareIndex += emptySquares;
            }
            else
            {
                int pieceID = BoardUtil.CharToInt(c);
                
                if (squareIndex < 64)
                {
                    pieces[squareIndex] = pieceID;
                    squareIndex++;
                }
            }
        }
        return pieces;
    }

    
}