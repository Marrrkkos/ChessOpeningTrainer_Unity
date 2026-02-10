using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static Game;

public class Pawn : Piece
{
    override
    public char pieceChar
    { get; } = 'P';
    override
    public bool hasMoved
    { get; set; } = false;
    override
    public int id { get; } = 0;
    override
    public bool color { get; }
    override
    public int position { get; set; }

    private Piece promotedPiece;
    public Pawn(Board board, bool color, int position) : base(board)
    {
        this.color = color;
        hasMoved = false;
        this.position = position;
    }
    override
    public List<Vector2Int> getPossibleMoves()
    {
        bool rotation = this.board.rotation;
        List<Move> currentMoves = this.board.currentGame.playedMoves;

        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        Vector2Int pos = BoardUtil.IndexToPos(position);
        int direction = (color == rotation) ? -1 : 1;

        Vector2Int[] offSets = { 
            new Vector2Int(0,1) * direction + pos, new Vector2Int(0, 2) * direction + pos,
            new Vector2Int(1, 1) * direction + pos , new Vector2Int(-1, 1) * direction + pos
        };

        if (BoardUtil.IsOnBoard(offSets[0]) && (this.board.fields[BoardUtil.PosToIndex(offSets[0])].piece == null))
        {
            if (direction == 1 && offSets[0].y == 7 || direction == -1 && offSets[0].y == 0)
            {
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[0]), 5));
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[0]), 6));
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[0]), 7));
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[0]), 8));
            }
            else {
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[0]), 0));
            }




            if (!hasMoved && (this.board.fields[BoardUtil.PosToIndex(offSets[1])].piece == null))
            {
                possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[1]), 0));
            }
        }
        if (BoardUtil.IsOnBoard(offSets[2]) && (this.board.fields[BoardUtil.PosToIndex(offSets[2])].piece != null))
        {
            if (this.board.fields[BoardUtil.PosToIndex(offSets[2])].piece.color != color)
            {
                if (direction == 1 && offSets[0].y == 7 || direction == -1 && offSets[0].y == 0)
                {
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[2]), 5));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[2]), 6));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[2]), 7));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[2]), 8));

                }
                else
                {
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[2]), 0));
                }
            }
        }
        if (BoardUtil.IsOnBoard(offSets[3]) && (this.board.fields[BoardUtil.PosToIndex(offSets[3])].piece != null))
        {
            if (this.board.fields[BoardUtil.PosToIndex(offSets[3])].piece.color != color)
            {
                if (direction == 1 && offSets[0].y == 7 || direction == -1 && offSets[0].y == 0)
                {
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[3]), 5));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[3]), 6));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[3]), 7));
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[3]), 8));
                }
                else
                {
                    possibleMoves.Add(new Vector2Int(BoardUtil.PosToIndex(offSets[3]), 0));
                }
            }
        }

        // En Passant
        if (currentMoves.Count > 0)
        {
            Move lastMove = currentMoves[currentMoves.Count - 1];

            if (lastMove.movedPiece.id == 0 && Mathf.Abs(lastMove.from - lastMove.to) == 16)
            {
                Vector2Int enemyPawnPos = BoardUtil.IndexToPos(lastMove.to);
                if (enemyPawnPos.y == pos.y && Mathf.Abs(enemyPawnPos.x - pos.x) == 1)
                {
                    int targetIndex = BoardUtil.PosToIndex(new Vector2Int(enemyPawnPos.x, pos.y + direction));
                    possibleMoves.Add(new Vector2Int(targetIndex, 3));
                }
            }
        }



        return possibleMoves;
    }

    override
    public Move doSpecialMove(int finalPos, int specialRule, bool refreshGUI)
    {
        Game game = this.board.currentGame;
        int currentPos = position;
        // EnPasant
        if (specialRule == 3) {
            Piece capturedPiece;

            // SET PIECES IN BOARD
            if (finalPos < currentPos)
            {
                capturedPiece = this.board.fields[finalPos + 8].piece;
                this.board.fields[finalPos + 8].setPiece(null, refreshGUI);
                this.board.fields[finalPos].setPiece(this, refreshGUI);
            }
            else {
                capturedPiece = this.board.fields[finalPos - 8].piece;
                this.board.fields[finalPos - 8].setPiece(null, refreshGUI);
                this.board.fields[finalPos].setPiece(this, refreshGUI);
            }
            this.board.fields[currentPos].setPiece(null, refreshGUI);


            // SET NEW POSITION
            this.position = finalPos;

            // SET PIECES IN PIECES
            if (capturedPiece != null)
            {
                if (color) { this.board.blackPieces.Remove(capturedPiece); } else { this.board.whitePieces.Remove(capturedPiece); }
            }

            // ADD MOVE TO GAME
            Move move = new Move (currentPos, finalPos, this, capturedPiece, specialRule, this.hasMoved, false, false);
            game.playedMoves.Add(move);
            return move; 
        }

        // DoPromotion
        if (specialRule >= 5)
        {
            Piece[] pieces = new Piece[4] {new Queen(this.board, color, finalPos), new Knight(this.board, color, finalPos), new Rook(this.board, color, finalPos), new Bishop(this.board, color, finalPos)};
            Piece capturedPiece = this.board.fields[finalPos].piece;
            promotedPiece = pieces[specialRule-5];

            // SET PIECES IN BOARD
            this.board.fields[finalPos].setPiece(promotedPiece, refreshGUI);
            this.board.fields[currentPos].setPiece(null, refreshGUI);

            // SET POSITION
            this.position = finalPos;

            // SET PIECES IN PIECES
            if (capturedPiece != null)
            {
                if (color)
                {
                    this.board.blackPieces.Remove(capturedPiece);
                }
                else
                {
                    this.board.whitePieces.Remove(capturedPiece);
                }
            }
            if (color)
            {
                this.board.whitePieces.Remove(this);
                this.board.whitePieces.Add(promotedPiece);
            }
            else
            {
                this.board.blackPieces.Remove(this);
                this.board.blackPieces.Add(promotedPiece);
            }

            Move move = new Move(currentPos, finalPos, this, capturedPiece, specialRule, this.hasMoved, false, false);
            // ADD MOVE TO GAME
            game.playedMoves.Add(move);
            return move;
        }

        return null;
    }

    override
    public Move undoSpecialMove(bool refreshGUI)
    {
        Game game = this.board.currentGame;

        if (game.playedMoves.Count == 0) return null;


        Move move = game.playedMoves[game.playedMoves.Count - 1];

        if (move.specialRule == 3)
        {
            this.board.fields[move.from].setPiece(this, refreshGUI);
            this.board.fields[move.to].setPiece(null, refreshGUI);

            if (move.to < move.from) {
                this.board.fields[move.to + 8].setPiece(move.capturedPiece, refreshGUI);
            } else {
                this.board.fields[move.to - 8].setPiece(move.capturedPiece, refreshGUI);
            }
                
        }
        else if (move.specialRule >= 4) {
            this.board.fields[move.from].setPiece(this, refreshGUI);
            this.board.fields[move.to].setPiece(move.capturedPiece, refreshGUI);
            if (color) { 
                this.board.whitePieces.Add(this);
                this.board.whitePieces.Remove(promotedPiece);
            } 
            else { 
                this.board.blackPieces.Add(this);
                this.board.blackPieces.Remove(promotedPiece);
            }
        }

        hasMoved = move.oldHasMoved;
        game.playedMoves.RemoveAt(game.playedMoves.Count - 1);

        this.position = move.from;
        if (move.capturedPiece != null)
        {
            if (color) { this.board.blackPieces.Add(move.capturedPiece); } else { this.board.whitePieces.Add(move.capturedPiece); }
        }

        return move;
    }

}

