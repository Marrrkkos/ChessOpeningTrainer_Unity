using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
public class Board : MonoBehaviour
{   
    [Header("Current Objective")]
    public Game currentGame;
    public Opening opening;

    public GameController gameController;
    
    
    [Header("Util")]
    public AnimationHandler animationHandler;
    public DrawOnBoard drawOnBoard;
    public bool rotation = false;

    [Header("BoardDesign")]
    public PieceSet pieceSet;
    public Color boardColor1;
    public Color boardColor2;

    // ActionServices
    public ActionService actionService;
    // Fields
    public Field[] fields;
    // Pieces
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

        //LOAD DEFAULT GAME

        currentGame = new Game(new Player[] { new Player("Player 1", 0, true), new Player("Player 2", 0, false) });
    }
    public Move UndoMove(bool refreshGUI, bool animation){

        if (currentGame.playedMoves.Count == 0) { return null;}
        Piece piece = currentGame.playedMoves[currentGame.playedMoves.Count - 1].movedPiece;
        Move move = piece.undoMove(refreshGUI, animation);

        currentGame.movesMemory.Add(move);

        NextTurn(refreshGUI);
        return move;
    }
    public bool DoSimpleMove(Game.SimpleMove simpleMove, bool refreshGUI,bool animation) {
        //Debug.Log("specialrule: " + simpleMove.specialRule + " field1: " + getString(simpleMove.from) + " field2: " + getString(simpleMove.to) + " san: " + simpleMove.san);
        return DoMove(simpleMove.specialRule, BoardUtil.IndexToString(simpleMove.from), BoardUtil.IndexToString(simpleMove.to), refreshGUI,animation);
    }
    public bool DoMove(Move move, bool refreshGUI,bool animation)
    {
        string m1 = BoardUtil.IndexToString(move.from);
        string m2 = BoardUtil.IndexToString(move.to);
        return DoMove(move.specialRule, m1, m2, refreshGUI,animation);
    }
    //public bool doSANMove(string san, bool refreshGUI) {
    //        return doSimpleMove(SAN_Handler.SANToMove(this, san, currentGame.players[currentGame.currentPlayer].color, true), refreshGUI);
    //}
    public bool DoMove(int specialRule, string m1, string m2, bool refreshGUI, bool animation)
    {
        int fieldID_1 = BoardUtil.StringToIndex(m1);
        int fieldID_2 = BoardUtil.StringToIndex(m2);
        Piece piece = this.fields[fieldID_1].piece;

        if (piece == null) {
            Debug.Log("Piece is zero!");
            return false;
        }

        if (piece.color != currentGame.players[currentGame.currentPlayer].color) {
            Debug.Log("Piece is Wrong Color!");
            return false;
        }

        Move move = piece.doMove(fieldID_2, specialRule, GetPossible(m1, refreshGUI), refreshGUI, animation);
        
        move.check = GameRules.checkOchecks(this, !piece.color);
        move.checkMate = GameRules.checkCheckMate(this, !piece.color);
        //move.san = SAN_Handler.MoveToSAN(this, move);


        if (currentGame.movesMemory.Count != 0)
        {
            Move m = currentGame.movesMemory.Last();
            if (m.Equals(move))
            {
                currentGame.movesMemory.Remove(m);
            }
            else
            {
                currentGame.movesMemory.Clear();
            }
        }

        NextTurn(refreshGUI);
        return true;
    }
    public List<Vector2Int> GetPossible(string m1, bool refreshGUI)
    {
        int fieldID = BoardUtil.StringToIndex(m1);
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
    
    private void NextTurn(bool refreshGUI)
    {
        //Move move = currentGame.moves[currentGame.moves.Count - 1];
        //string san = BoardUtil.MoveToSAN(this, move);
        //currentGame.moves[currentGame.moves.Count - 1].san = san;

        int x = this.currentGame.currentPlayer;
        this.currentGame.currentPlayer = (x + 1) % 2;
        this.drawOnBoard.refreshPossibles(refreshGUI);
        if(refreshGUI){
            this.drawOnBoard.DrawLastMove();
        }

        if(gameController != null)
            gameController.OnMoveDone(opening, currentGame);
           
    }



    public void ResetBoard(bool refreshGUI)
    {
        for (int i = 0; i < 64; i++)
        {
            fields[i].SetPiece(null, true);
        }
        whitePieces.Clear();
        blackPieces.Clear();
        whiteKing = null;
        blackKing = null;

        currentGame.playedMoves.Clear();
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            fields[i+8].SetPiece(new Pawn(this,false,i + 8), refreshGUI);
        }
        for (int i = 0; i < 8; i++)
        {
            fields[i + 48].SetPiece(new Pawn(this,true,i + 48), refreshGUI);
        }
        fields[0].SetPiece(new Rook(this,false,0), refreshGUI);
        fields[1].SetPiece(new Knight(this,false,1), refreshGUI);
        fields[2].SetPiece(new Bishop(this,false,2), refreshGUI);
        fields[3].SetPiece(new Queen(this,false,3), refreshGUI);
        fields[4].SetPiece(new King(this,false,4), refreshGUI);
        fields[5].SetPiece(new Bishop(this,false,5), refreshGUI);
        fields[6].SetPiece(new Knight(this,false,6), refreshGUI);
        fields[7].SetPiece(new Rook(this,false,7), refreshGUI);

        fields[56].SetPiece(new Rook(this,true,56), refreshGUI);
        fields[57].SetPiece(new Knight(this,true,57), refreshGUI);
        fields[58].SetPiece(new Bishop(this,true,58), refreshGUI);
        fields[59].SetPiece(new Queen(this,true,59), refreshGUI);
        fields[60].SetPiece(new King(this,true,60), refreshGUI);
        fields[61].SetPiece(new Bishop(this,true,61), refreshGUI);
        fields[62].SetPiece(new Knight(this,true,62), refreshGUI);
        fields[63].SetPiece(new Rook(this,true,63), refreshGUI);

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
        if(refreshGUI){
            for (int i = 16; i < 48; i++)
            {
                fields[i].pieceImage.gameObject.SetActive(false);
            }
        }

        currentGame.currentPlayer = 0;
        drawOnBoard.DrawLastMove();
    }

    
}