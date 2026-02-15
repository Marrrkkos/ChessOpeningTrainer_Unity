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
    public abstract Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI);

    public abstract Move undoSpecialMove(bool refreshGUI);
    

    
    
    public Move doMove(int finalPos, int specialRule, List<Vector2Int> possibleMoves, bool refreshGUI) {

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
            return this.doSpecialMove(finalPos, specialRule, refreshGUI);
        }
        int currentPos = this.position;

        Piece capturedPiece = this.board.fields[finalPos].piece;


        // SET PIECES IN BOARD
        this.board.fields[finalPos].setPiece(this, refreshGUI);
        this.board.fields[currentPos].setPiece(null, refreshGUI);



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
    public Move undoMove(bool refreshGUI) {
        Game game = this.board.currentGame;
        if (game.playedMoves.Count == 0) return null;

        Move move = game.playedMoves[game.playedMoves.Count - 1];

        if (move.specialRule != 0)
        {
            return this.undoSpecialMove(refreshGUI);
        }

        // UNDO MOVE IN GAME
        game.playedMoves.RemoveAt(game.playedMoves.Count - 1);

        // UNDO PIECES IN BOARD
        this.board.fields[move.from].setPiece(this, refreshGUI);
        this.board.fields[move.to].setPiece(move.capturedPiece, refreshGUI);

        

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
