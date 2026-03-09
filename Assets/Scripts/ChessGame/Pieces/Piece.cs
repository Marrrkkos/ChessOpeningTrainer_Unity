using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public abstract class Piece
{
    [JsonIgnore] public Board board;

    public Piece(Board board) { 
        this.board = board;
    }
    public abstract char pieceChar { get; }
    public abstract bool hasMoved { get; set; }
    public abstract int id { get; }
    public abstract int position { get; set; }
    public abstract bool color { get; }
    public abstract List<Vector2Int> getPossibleMoves();
    public abstract Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI, bool animation);

    public abstract Move undoSpecialMove(bool refreshGUI, bool animation);
    

    
    
    public Move doMove(int finalPos, int specialRule, List<Vector2Int> possibleMoves, bool refreshGUI, bool animation) {

        Game game = this.board.currentGame;

        if (possibleMoves != null)
        {
            if (!possibleMoves.Contains(new Vector2Int(finalPos, specialRule)))
            {
                Debug.Log("Move Not Possible");
                return null;
            }
        }

        if (specialRule != 0) {
            return this.doSpecialMove(finalPos, specialRule, refreshGUI, animation);
        }
        int currentPos = this.position;

        Piece capturedPiece = this.board.fields[finalPos].piece;

        
        // SET PIECES IN BOARD
        if (animation)
        {
            this.board.fields[finalPos].piece = this;
            this.board.fields[currentPos].piece = null;
            int[] refreshIDS = new int[]{currentPos, finalPos};
            board.animationHandler.DoAnimation(this, currentPos, finalPos, refreshIDS);
        }
        else{
            this.board.fields[finalPos].SetPiece(this, refreshGUI);
            this.board.fields[currentPos].SetPiece(null, refreshGUI);
        }


        // SET NEW POSITION
        this.position = finalPos;

        // SET PIECES IN PIECES
        if (capturedPiece != null)
        {
            if (color) { this.board.blackPieces.Remove(capturedPiece); } else { this.board.whitePieces.Remove(capturedPiece); }
        }
        Move move = new Move(currentPos, finalPos, this, capturedPiece, 0, this.hasMoved, false, false);
        // ADD MOVE TO GAME
        game.playedMoves.Add(move);


        //SET HASMOVED
        this.hasMoved = true;


        return move;
    }
    public Move undoMove(bool refreshGUI, bool animation) {
        Game game = this.board.currentGame;
        if (game.playedMoves.Count == 0) return null;

        Move move = game.playedMoves[game.playedMoves.Count - 1];

        if (move.specialRule != 0)
        {
            return this.undoSpecialMove(refreshGUI, animation);
        }

        // UNDO MOVE IN GAME
        game.playedMoves.RemoveAt(game.playedMoves.Count - 1);

        
        // UNDO PIECES IN BOARD
        if (animation)
        {
            this.board.fields[move.from].piece = this;
            this.board.fields[move.to].piece = null;
            int[] refreshIDS = new int[]{move.from, move.to};
            board.animationHandler.DoAnimation(this, move.to, move.from, refreshIDS);
        }
        else
        {
            this.board.fields[move.from].SetPiece(this, refreshGUI);
            this.board.fields[move.to].SetPiece(move.capturedPiece, refreshGUI);
        }

        

        // SET LAST POSITION
        this.position = move.from;


        // UNDO PIECES IN PIECES
        if (move.capturedPiece != null)
        {
            if (color) { this.board.blackPieces.Add(move.capturedPiece); } else { this.board.whitePieces.Add(move.capturedPiece); }
        }

        // UNDO HASMOVED
        this.hasMoved = move.oldHasMoved;


        return move;
    }
}
