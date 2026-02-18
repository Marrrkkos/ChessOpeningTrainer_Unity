using UnityEngine;
using System.Text;
using System;

public class FEN_Handler
{
    // Hilfsstruktur, um alle Daten einer FEN zu halten
    public struct FenData
    {
        public int[] pieceIds;      // IDs (0=Pawn, 1=Bishop, etc.)
        public int[] pieceColors;   // 0=White, 1=Black (oder -1 für leer)
        public bool isWhiteTurn;    // Wer ist am Zug?
        public string castlingRights; // z.B. "KQkq" oder "-"
        public string enPassant;    // z.B. "e3" oder "-"
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

    /// <summary>
    /// Erstellt einen FEN-String basierend auf dem aktuellen Board-Zustand.
    /// </summary>
    public static string BoardToFEN(Board board)
    {
        StringBuilder fen = new StringBuilder();

        // 1. Piece Placement (Figurenplatzierung)
        for (int i = 0; i < 8; i++) // Rank 8 bis Rank 1 (Index 0 bis 7 basierend auf BoardUtil Logik)
        {
            int emptyCount = 0;

            for (int j = 0; j < 8; j++) // File a bis File h
            {
                int fieldIndex = i * 8 + j;
                // Sicherheitscheck, falls das Board-Array kleiner wäre (optional)
                if(fieldIndex >= board.fields.Length) break;

                var field = board.fields[fieldIndex];

                if (field.piece != null)
                {
                    // Wenn wir leere Felder gezählt haben, schreiben wir die Zahl jetzt
                    if (emptyCount > 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    // Bestimme Farbe: true für Weiß (Upper), false für Schwarz
                    bool isWhite = field.piece.color; 
                    char pieceChar = BoardUtil.IdToChar(field.piece.id, isWhite);
                    
                    fen.Append(pieceChar);
                }
                else
                {
                    emptyCount++;
                }
            }

            // Restliche leere Felder am Ende der Zeile
            if (emptyCount > 0)
            {
                fen.Append(emptyCount);
            }

            // Trennzeichen '/' zwischen den Zeilen (nicht nach der letzten Zeile)
            if (i != 7)
            {
                fen.Append('/');
            }
        }

        // 2. Active Color (Wer ist am Zug)
        string color = (board.currentGame.currentPlayer == WHITE) ? "w" : "b";
        fen.Append(" " + color);

        // 3. Castling Rights (Rochade)
        // HINWEIS: Hier müsstest du Zugriff auf deine Rochade-Variablen im Board haben.
        // Da diese im Snippet nicht sichtbar sind, habe ich eine Platzhalter-Logik eingefügt.
        // Du solltest dies mit deinen echten Variablen (z.B. board.whiteCastleKing) ersetzen.
        string castling = GetCastlingString(board); 
        fen.Append(" " + castling);

        // 4. En Passant Target Square
        // HINWEIS: Auch hier Zugriff auf das Board nötig.
        string enPassant = GetEnPassantString(board);
        fen.Append(" " + enPassant);

        // 5. Halfmove Clock (50-Züge-Regel)
        // Standardmäßig 0, wenn nicht im Board gespeichert
        fen.Append(" 0"); 

        // 6. Fullmove Number
        // Standardmäßig 1, wenn nicht im Board gespeichert
        fen.Append(" 1");

        return fen.ToString();
    }

    /// <summary>
    /// Liest einen FEN-String und gibt alle notwendigen Daten zurück (Figuren, Farben, Spielstatus).
    /// </summary>
    public static FenData FENToPos(string fen)
    {
        FenData data = new FenData();
        data.pieceIds = new int[64];
        data.pieceColors = new int[64];
        
        // Init Arrays
        for(int k = 0; k < 64; k++) 
        {
            data.pieceIds[k] = EMPTY_SQUARE_ID;
            data.pieceColors[k] = -1; 
        }

        // FEN String in seine 6 Teile zerlegen
        string[] sections = fen.Split(' ');

        // --- Teil 1: Figurenplatzierung ---
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
                // Hier ist die wichtige Änderung: Wir prüfen Groß/Kleinschreibung für die Farbe
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

        // --- Teil 2: Aktive Farbe ---
        if (sections.Length > 1)
        {
            data.isWhiteTurn = (sections[1] == "w");
        }
        else
        {
            data.isWhiteTurn = true; // Default
        }

        // --- Teil 3: Rochaderechte ---
        if (sections.Length > 2)
        {
            data.castlingRights = sections[2];
        }
        else
        {
            data.castlingRights = "-";
        }

        // --- Teil 4: En Passant ---
        if (sections.Length > 3)
        {
            data.enPassant = sections[3];
        }
        else
        {
            data.enPassant = "-";
        }
        
        // (Teil 5 & 6 für Clocks sind optional für die reine Positionierung, aber könnten hier geparst werden)

        return data;
    }

    // --- Helper Methoden für BoardToFEN (Platzhalter) ---

    private static string GetCastlingString(Board board)
    {
        // TODO: Verbinde dies mit deinen echten boolschen Variablen im Board!
        // Beispiel:
        // StringBuilder sb = new StringBuilder();
        // if (board.whiteCastleKing) sb.Append('K');
        // if (board.whiteCastleQueen) sb.Append('Q');
        // if (board.blackCastleKing) sb.Append('k');
        // if (board.blackCastleQueen) sb.Append('q');
        // return sb.Length > 0 ? sb.ToString() : "-";
        
        return "KQkq"; // Temporärer Default, damit es funktioniert
    }

    private static string GetEnPassantString(Board board)
    {
        // TODO: Wenn dein Board ein Feld für EnPassant hat (z.B. int enPassantIndex),
        // wandle es hier um: return BoardUtil.IndexToString(board.enPassantIndex);
        
        return "-"; 
    }
}