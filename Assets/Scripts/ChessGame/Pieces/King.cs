using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class King : Piece
{
    override
    public char pieceChar
    { get; } = 'K';
    override
    public bool hasMoved
    { get; set; } = false;
    override
    public int id{ get; } = 5;
    override
    public bool color { get; }
    override
    public int position { get; set; }
    public Sprite sprite;
    

    public King(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        hasMoved = false;
        this.position = position;

    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        int currentPos = this.position;

        int currentRow = currentPos / 8;
        int currentCol = currentPos % 8;


        // Alle 8 Richtungen, aber nur 1 Feld weit
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue; // Das eigene Feld �berspringen

                int targetRow = currentRow + y;
                int targetCol = currentCol + x;

                if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
                {
                    int targetIdx = targetRow * 8 + targetCol;
                    Piece p = this.board.fields[targetIdx].piece;

                    if (p == null || p.color != this.color)
                    {
                        possibleMoves.Add(new Vector2Int(targetIdx, 0));
                    }
                }
            }
        }

        //Short Castle
        if (!hasMoved) {
            if (this.board.fields[currentPos + 3].piece != null) {
                if (this.board.fields[currentPos + 1].piece == null && this.board.fields[currentPos + 2].piece == null && this.board.fields[currentPos + 3].piece.id == 2 && !this.board.fields[currentPos + 3].piece.hasMoved) {
                    if (!GameRules.checkOchecks(board, color))
                    {
                        doMove(currentPos + 1, 0, null, false, false);

                        if (!GameRules.checkOchecks(board, color))
                        {
                            possibleMoves.Add(new(currentPos + 2, 1));
                        }
                        undoMove(false, false);
                    }
                }
            }
            //Long Castle
            if (this.board.fields[currentPos - 4].piece != null)
            {
                if (this.board.fields[currentPos - 1].piece == null && this.board.fields[currentPos - 2].piece == null && this.board.fields[currentPos - 3].piece == null && this.board.fields[currentPos - 4].piece.id == 2 && !this.board.fields[currentPos - 4].piece.hasMoved)
                {
                    if (!GameRules.checkOchecks(board, color))
                    {
                        doMove(currentPos - 1, 0, null, false, false);
                        if (!GameRules.checkOchecks(board, color))
                        {
                            possibleMoves.Add(new(currentPos - 2, 2));
                        }
                        undoMove(false, false);
                    }
                }
            } 
        }


        return possibleMoves;
    }
    override
    public Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI, bool animation)
    {
        
        Game game = this.board.currentGame;
        int currentPos = position;
        //Short Castle
        if (specialRule == 1) {
            // SET PIECES IN BOARD
            if (animation)
            {   
                this.board.fields[currentPos + 2].piece = this;
                this.board.fields[currentPos].piece = null;
                
                int[] refreshIDs = new int[]{currentPos, currentPos + 2};
                board.animationHandler.DoAnimation(this, currentPos, currentPos+2, refreshIDs);

                this.board.fields[currentPos + 1].piece = this.board.fields[currentPos + 3].piece;
                this.board.fields[currentPos + 3].piece = null;
                
                refreshIDs = new int[]{currentPos + 1, currentPos + 3};
                board.animationHandler.DoAnimation(this.board.fields[currentPos + 1].piece, currentPos+3, currentPos+1, refreshIDs);
            }
            else{
                this.board.fields[currentPos + 2].SetPiece(this, refreshGUI);
                this.board.fields[currentPos + 1].SetPiece(this.board.fields[currentPos + 3].piece, refreshGUI);
                this.board.fields[currentPos + 3].SetPiece(null, refreshGUI);
                this.board.fields[currentPos].SetPiece(null, refreshGUI);
            }

            // SET NEW POSITION
            this.board.fields[currentPos + 1].piece.position = currentPos + 1;
            position = finalPos;

            // ADD MOVE TO GAME
            Move move = new Move(currentPos, finalPos, this, null, specialRule, this.hasMoved, false, false);
            game.playedMoves.Add(move);
            
            // SET HAS MOVED
            this.hasMoved = true;
            this.board.fields[currentPos + 1].piece.hasMoved = true;

            return move;
        }
        else if(specialRule == 2) {  //Long Castle

            // SET PIECES IN BOARD
            if (animation)
            {
                this.board.fields[currentPos - 2].piece = this;
                this.board.fields[currentPos].piece = null;

                int[] refreshIDs = new int[]{currentPos, currentPos - 2};
                board.animationHandler.DoAnimation(this, currentPos, currentPos-2, refreshIDs);

                this.board.fields[currentPos - 1].piece = this.board.fields[currentPos - 4].piece;
                this.board.fields[currentPos - 4].piece = null;
                refreshIDs = new int[]{currentPos - 1, currentPos - 4};
                board.animationHandler.DoAnimation(this.board.fields[currentPos - 1].piece, currentPos-4, currentPos-1, refreshIDs);
            }else{
                this.board.fields[currentPos - 2].SetPiece(this, refreshGUI);
                this.board.fields[currentPos - 1].SetPiece(this.board.fields[currentPos - 4].piece, refreshGUI);
                this.board.fields[currentPos - 4].SetPiece(null, refreshGUI);
                this.board.fields[currentPos].SetPiece(null, refreshGUI);
            }

            // SET NEW POSITION
            this.board.fields[currentPos - 1].piece.position = currentPos - 1;
            position = finalPos;



            // ADD MOVE TO GAME
            Move move = new Move(currentPos, finalPos, this, null, specialRule, this.hasMoved, false, false);
            game.playedMoves.Add(move);

            // SET HAS MOVED
            this.hasMoved = true;
            this.board.fields[currentPos - 1].piece.hasMoved = true;
            return move;
        }
        return null;
    }
    override
    public Move undoSpecialMove(bool refreshGUI, bool animation)       //Only for PossibleMoveSearch
    {
        Game game = this.board.currentGame;

        if (game.playedMoves.Count == 0) return null;
        
        Move move = game.playedMoves[game.playedMoves.Count - 1];

        if (move.specialRule == 1) {

            // UNDO PIECES IN BOARD
            if (animation)
            {
                this.board.fields[move.from].piece = this;
                this.board.fields[move.from + 2].piece = null;

                int[] refreshIDs = new int[]{move.from, move.from + 2};
                board.animationHandler.DoAnimation(this, move.from+2, move.from, refreshIDs);

                this.board.fields[move.from + 3].piece = this.board.fields[move.from + 1].piece;
                this.board.fields[move.from + 1].piece = null;
                refreshIDs = new int[]{move.from + 1, move.from + 3};
                board.animationHandler.DoAnimation(this.board.fields[move.from + 3].piece, move.from+1, move.from+3, refreshIDs);
            }else{
                this.board.fields[move.from].SetPiece(this, refreshGUI);
                this.board.fields[move.from + 3].SetPiece(this.board.fields[move.from + 1].piece, refreshGUI);
                this.board.fields[move.from + 2].SetPiece(null, refreshGUI);
                this.board.fields[move.from + 1].SetPiece(null, refreshGUI);
            }
            // SET LAST POSITION
            this.board.fields[move.from + 3].piece.position = move.from + 3;
            this.position = move.from;

            // UNDO MOVE IN GAME
            game.playedMoves.RemoveAt(game.playedMoves.Count - 1);

            // UNDO HASMOVED
            hasMoved = move.oldHasMoved;
            this.board.fields[move.from + 3].piece.hasMoved = false;

        }
        else if (move.specialRule == 2) 
        {
            // UNDO PIECES IN BOARD
            if (animation)
            {
                this.board.fields[move.from].piece = this;
                this.board.fields[move.from - 2].piece = null;

                int[] refreshIDs = new int[]{move.from, move.from - 2};
                board.animationHandler.DoAnimation(this, move.from-2, move.from, refreshIDs);

                this.board.fields[move.from - 4].piece = this.board.fields[move.from + 1].piece;
                this.board.fields[move.from - 1].piece = null;
                refreshIDs = new int[]{move.from - 1, move.from - 4};
                board.animationHandler.DoAnimation(this.board.fields[move.from - 4].piece, move.from-1, move.from-4, refreshIDs);
            }else{
                this.board.fields[move.from].SetPiece(this, refreshGUI);
                this.board.fields[move.from - 4].SetPiece(this.board.fields[move.from - 1].piece, refreshGUI);
                this.board.fields[move.from - 2].SetPiece(null, refreshGUI);
                this.board.fields[move.from - 1].SetPiece(null, refreshGUI);
            }
            // SET LAST POSITION
            this.board.fields[move.from - 4].piece.position = move.from - 4;
            this.position = move.from;
            // UNDO MOVE IN GAME
            game.playedMoves.RemoveAt(game.playedMoves.Count - 1);

            // UNDO HASMOVED
            hasMoved = move.oldHasMoved;
            this.board.fields[move.from - 4].piece.hasMoved = false;

        }
        return move;
    }
}
