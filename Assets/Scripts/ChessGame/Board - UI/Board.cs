using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Game currentGame;
    public Opening opening;
    public bool rotation = true;

    [Header("Util")]
    public ActionService actionService;
    public DrawOnBoard drawOnBoard;
    public Field[] fields;
    public PieceSet pieceSet;

    public Piece whiteKing;
    public Piece blackKing;

    public List<Piece> whitePieces = new List<Piece>();
    public List<Piece> blackPieces = new List<Piece>();

    //INITIALIZE
    public void Awake()
    {
        actionService = new ActionService(this);
        //LOAD BOARD

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            fields[i + 8].piece = new Pawn(this, false, i + 8);
        }
        for (int i = 0; i < 8; i++)
        {
            fields[i + 48].piece = new Pawn(this, true, i + 48);
        }
        fields[0].piece = new Rook(this, false, 0);
        fields[1].piece = new Knight(this, false, 1);
        fields[2].piece = new Bishop(this, false, 2);
        fields[3].piece = new Queen(this, false, 3);
        fields[4].piece = new King(this, false, 4);
        fields[5].piece = new Bishop(this, false, 5);
        fields[6].piece = new Knight(this, false, 6);
        fields[7].piece = new Rook(this, false, 7);

        fields[56].piece = new Rook(this, true, 56);
        fields[57].piece = new Knight(this, true, 57);
        fields[58].piece = new Bishop(this, true, 58);
        fields[59].piece = new Queen(this, true, 59);
        fields[60].piece = new King(this, true, 60);
        fields[61].piece = new Bishop(this, true, 61);
        fields[62].piece = new Knight(this, true, 62);
        fields[63].piece = new Rook(this, true, 63);

        whiteKing = fields[60].piece; //WHITE AND BLACK KING 
        blackKing = fields[4].piece;
        for (int i = 48; i < 64; i++)
        {
            whitePieces.Add(fields[i].piece);
        }
        for (int i = 0; i < 16; i++)
        {
            blackPieces.Add(fields[i].piece);
        }


        drawOnBoard.arrow.AddArrow(new Vector2(0, 0), new Vector2(100, 100), UnityEngine.Color.aliceBlue);
        drawOnBoard.arrow.AddArrow(new Vector2(200, 200), new Vector2(300, 300), UnityEngine.Color.red);
        //LOAD DEFAULT GAME

        currentGame = new Game(new Player[] { new Player("Player 1", 0, true), new Player("Player 2", 0, false) });
    }
    public bool undoMove(bool refreshGUI) {
        if (currentGame.moves.Count == 0) { return false;}
        Piece piece = currentGame.moves[currentGame.moves.Count - 1].movedPiece;
        piece.undoMove(refreshGUI);
        nextTurn(refreshGUI);
        return true;
    }
    public bool doSimpleMove(Game.SimpleMove simpleMove, bool refreshGUI) {
        //Debug.Log("specialrule: " + simpleMove.specialRule + " field1: " + getString(simpleMove.from) + " field2: " + getString(simpleMove.to) + " san: " + simpleMove.san);
        return doMove(simpleMove.specialRule, getString(simpleMove.from), getString(simpleMove.to), refreshGUI);
    }

    public bool doSANMove(string san, bool refreshGUI) {
        return doSimpleMove(BoardUtil.SANToMove(this, san, currentGame.players[currentGame.currentPlayer].color, true), refreshGUI);
    }
    public bool doMove(int specialRule, string m1, string m2, bool refreshGUI)
    {
        int fieldID_1 = this.getID(m1);
        int fieldID_2 = this.getID(m2);
        Piece piece = this.fields[fieldID_1].piece;

        if (piece == null) {
            Debug.Log("Piece is zero!");
            return false;
        }

        if (piece.color != currentGame.players[currentGame.currentPlayer].color) {
            Debug.Log("Piece is Wrong Color!");
            return false;
        }

        Move move = piece.doMove(fieldID_2, specialRule, getPossible(m1, refreshGUI), refreshGUI);
        
        move.check = GameRules.checkOchecks(this, !piece.color);
        move.checkMate = GameRules.checkCheckMate(this, !piece.color);
        move.san = BoardUtil.MoveToSAN(this, move);
        nextTurn(refreshGUI);
        return true;
    }
    public List<Vector2Int> getPossible(string m1, bool refreshGUI)
    {
        int fieldID = this.getID(m1);
        Piece piece = this.fields[fieldID].piece;

        if (piece == null)
        {
            Debug.Log("Piece is zero!");
            return new List<Vector2Int>();
        }

        if (piece.color != currentGame.players[currentGame.currentPlayer].color)
        {
            Debug.Log("Piece is Wrong Color!");
            return new List<Vector2Int>();
        }
        List<Vector2Int>  possibleMoves = new List<Vector2Int>();

        possibleMoves = piece.getPossibleMoves();
        possibleMoves = GameRules.removeChecks(fieldID, possibleMoves, this);
        this.drawOnBoard.drawPossibles(possibleMoves, refreshGUI);

        return possibleMoves;
    }
    
    private void nextTurn(bool refreshGUI)
    {
        //Move move = currentGame.moves[currentGame.moves.Count - 1];
        //string san = BoardUtil.MoveToSAN(this, move);
        //currentGame.moves[currentGame.moves.Count - 1].san = san;

        int x = this.currentGame.currentPlayer;
        this.currentGame.currentPlayer = (x + 1) % 2;
        this.drawOnBoard.refreshPossibles(refreshGUI);
    }




    public void reset()
    {
        for (int i = 0; i < 64; i++)
        {
            fields[i].setPiece(null, true);
        }
        whitePieces.Clear();
        blackPieces.Clear();
        whiteKing = null;
        blackKing = null;

        currentGame.moves.Clear();
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            fields[i + 8].piece = new Pawn(this, false, i + 8);
        }
        for (int i = 0; i < 8; i++)
        {
            fields[i + 48].piece = new Pawn(this, true, i + 48);
        }
        fields[0].piece = new Rook(this, false, 0);
        fields[1].piece = new Knight(this, false, 1);
        fields[2].piece = new Bishop(this, false, 2);
        fields[3].piece = new Queen(this, false, 3);
        fields[4].piece = new King(this, false, 4);
        fields[5].piece = new Bishop(this, false, 5);
        fields[6].piece = new Knight(this, false, 6);
        fields[7].piece = new Rook(this, false, 7);

        fields[56].piece = new Rook(this, true, 56);
        fields[57].piece = new Knight(this, true, 57);
        fields[58].piece = new Bishop(this, true, 58);
        fields[59].piece = new Queen(this, true, 59);
        fields[60].piece = new King(this, true, 60);
        fields[61].piece = new Bishop(this, true, 61);
        fields[62].piece = new Knight(this, true, 62);
        fields[63].piece = new Rook(this, true, 63);

        whiteKing = fields[60].piece; //WHITE AND BLACK KING 
        blackKing = fields[4].piece;
        for (int i = 48; i < 64; i++)
        {
            whitePieces.Add(fields[i].piece);
        }
        for (int i = 0; i < 16; i++)
        {
            blackPieces.Add(fields[i].piece);
        }

        currentGame.currentPlayer = 0;
    }
    private string GetShortName(Piece p)
    {
        return p.GetType().Name.Substring(0, 1).ToUpper();
    }

    // HELPER FUNCTIONS
    public int getID(string fieldName) {

        int letter = fieldName[0] - 'a';
        int number = fieldName[1] - '1';


        if (rotation)
        {
            return (7 - number) * 8 + letter;
        }
        else {
            return (7 - letter) * 8 + number;
        }
    }
    public string getString(int id)
    {
        int letterIndex;
        int numberIndex;

        if (rotation)
        {
            // Umkehrung von: (7 - number) * 8 + letter
            letterIndex = id % 8;
            numberIndex = 7 - (id / 8);
        }
        else
        {
            // Umkehrung von: (7 - letter) * 8 + number
            letterIndex = 7 - (id / 8);
            numberIndex = id % 8;
        }

        // Umwandlung der Zahlen (0-7) zurück in Zeichen ('a'-'h' und '1'-'8')
        char letter = (char)('a' + letterIndex);
        char number = (char)('1' + numberIndex);

        return $"{letter}{number}";
    }




    // DEBUG







    public void checkSynchro() {
        for (int i = 0; i < 64; i++)
        {
            if(fields[i].piece != null){

                if (i != fields[i].piece.position) {
                    Debug.Log("SYNCHROERROR    -   DU ARRRRSSSCHHHHHHLLLOOOOOCCCCHHHHHHHHHHH");
                }
            } 
        }
    }
}