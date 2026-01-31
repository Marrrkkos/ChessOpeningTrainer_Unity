using System.Collections.Generic;
using UnityEngine;

public class ActionService
{
    private Board board;

    private string m1 = "";
    private string m2 = "";

    private string selectedField = "";

    private bool promotion = false;

    private List<Vector2Int> possibleMoves = new List<Vector2Int>();

    private int[] promotionIndexes = new int[4];

    public ActionService(Board board) { 
        this.board = board;
    }
    public void setFieldOnMouseDown(string fieldName)
    {
        if (promotion) {
            doPromotion(fieldName);
            return;
        }


        if (m1 == "")
        {
            m1 = fieldName;
            if (isCorrectPiece())
            {
                possibleMoves = board.getPossible(m1, true);
            }
            else
            {
                refreshTurn();
            }
        }
        else
        {
            m2 = fieldName;
            int rule = isCorrectMove();
            if (rule == -1)
            {
                refreshTurn();
            } else if(rule == -2) {

                string x = m2;
                refreshTurn();
                m1 = x;
                possibleMoves = board.getPossible(m1, true);
            }else if  (rule >= 4) {
                showPromotion();
            }
            else
            {
                board.doMove(rule, m1, m2, true);
                refreshTurn();
            }
        }

    }
    public void setFieldOnMouseUp(string fieldName)
    {
        if (promotion)
        {
            return;
        }
        if (m1 == "")
        {
            return;
        }
        m2 = fieldName;

        if (m1.Equals(m2))
        {
            return;
        }

        int rule = isCorrectMove();
        if (rule > 0)
        {
            board.doMove(rule, m1, m2, true);
            refreshTurn();
        }
        else
        {
            refreshTurn();
        }
    }
    public void showPromotion()
    {
        promotion = true;
        bool rotation = board.rotation;
        int fieldID = board.getID(m2);
        Piece piece = board.fields[board.getID(m1)].piece;


        setPiecesActive(board, false);

        int direction = (rotation == piece.color) ? 1 : -1;


        for (int i = 3; i >= 0; i--)
        {

            promotionIndexes[i] = fieldID + (direction * i * 8);
            board.fields[promotionIndexes[i]].setPieceImage(4 - i, piece.color);
        }
        board.drawOnBoard.refreshPossibles(true);
    }
    public void doPromotion(string fieldName)
    {
        promotion = false;
        int fieldID = board.getID(fieldName);
        for (int i = 0; i < 4; i++)
        {
            Field field = board.fields[promotionIndexes[i]];
            if (field.piece != null)
            {
                field.setPieceImage(field.piece.id, field.piece.color);
            }
            else
            {
                field.setPieceImage(-1, false);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (promotionIndexes[i] == fieldID)
            {
                board.doMove(5 + i, m1, m2, true);
            }
        }
        setPiecesActive(board, true);
    }

    private bool isCorrectPiece()
    {
        Player player = board.currentGame.players[board.currentGame.currentPlayer];
        int fieldID = board.getID(m1);
        Piece piece = board.fields[fieldID].piece;
        if (piece == null)
        {
            return false;
        }
        if (piece.color != player.color)
        {
            return false;
        }
        return true;
    }
    private int isCorrectMove() // return -1 bei fail und sonst die specialrule
    {
        Player player = board.currentGame.players[board.currentGame.currentPlayer];
        int fieldID = board.getID(m2);
        Piece piece = board.fields[fieldID].piece;
        if (piece != null) {
            if (piece.color == player.color) {
                return -2;
            }
        }
        if (possibleMoves.Count == 0)
        {
            return -1;
        }
        foreach (Vector2Int pm in possibleMoves)
        {
            if (board.getString(pm.x).Equals(m2)) {
            return pm.y; }
        }
        return -1;
    }



    
    
    private void setPiecesActive(Board board, bool active) {
        foreach (Piece p in board.whitePieces)
        {
            Field field = board.fields[p.position];
            field.pieceImage.gameObject.SetActive(active);
            field.refreshImage(p.position);
        }
        foreach (Piece p in board.blackPieces)
        {
            Field field = board.fields[p.position];
            field.pieceImage.gameObject.SetActive(active);
            field.refreshImage(p.position);
        }
    }
    private void refreshTurn()
    {
        m1 = "";
        m2 = "";
        board.drawOnBoard.refreshPossibles(true);
    }
}
