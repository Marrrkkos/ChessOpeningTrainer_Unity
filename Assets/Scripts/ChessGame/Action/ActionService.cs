using System.Collections.Generic;
using UnityEngine;

public class ActionService
{
    private Board board;

    private string m1 = "";
    private string m2 = "";
    public int selectedField = 0;
    
    private bool promotion = false;

    private List<Vector2Int> possibleMoves = new List<Vector2Int>();

    private int[] promotionIndexes = new int[4];

    public ActionService(Board board) { 
        this.board = board;
    }
    public void setFieldOnMouseDown(string fieldName)
    {
        if (promotion) {
            Debug.Log("DoPromotionDown - Down");
            doPromotion(fieldName);
            return;
        }


        if (m1 == "")
        {
            m1 = fieldName;
            if (isCorrectPiece())
            {
                Debug.Log("CorrectPiece, GetPossible - Down");
                possibleMoves = board.getPossible(m1, true);
            }
            else
            {
                Debug.Log("NotCorrectPiece - Down");
                refreshTurn(false);
            }
        }
        else
        {
            m2 = fieldName;
            int rule = isCorrectMove();
            if (rule == -1)
            {
                Debug.Log("NoPossibles - Down");
                refreshTurn(false);
            } else if(rule == -2) {
                Debug.Log("OwnColor - Down");
                string x = m2;
                refreshTurn(false);
                m1 = x;
                possibleMoves = board.getPossible(m1, true);
            }else if  (rule >= 4) {
                Debug.Log("Promotion - Down");
                showPromotion();
            }
            else
            {
                Debug.Log("Move - Down");
                board.doMove(rule, m1, m2, true, true, false);
                refreshTurn(true);
            }
        }

    }
    public void setFieldOnMouseUp(string fieldName)
    {
        if (promotion)
        {
            Debug.Log("Promotion - UP");
            return;
        }
        if (m1 == "")
        {
            Debug.Log("m1Empty - UP");
            return;
        }
        m2 = fieldName;

        if (m1.Equals(m2))
        {
            Debug.Log("Equals - UP");
            return;
        }

        int rule = isCorrectMove();
        if (rule >= 0)
        {
            Debug.Log("Move - UP");
            board.doMove(rule, m1, m2, true, true, false);
            
            refreshTurn(true);
        }
        else
        {
            if(rule == -1)
            {
                Debug.Log("NoPossibles - UP");
            }
            else if( rule == -2)
            {
                Debug.Log("OwnColor - Up");
            }
            refreshTurn(false);
        }
    }
    public void showPromotion()
    {
        promotion = true;
        int fieldID = BoardUtil.StringToIndex(m2);
        Piece piece = board.fields[BoardUtil.StringToIndex(m1)].piece;


        setPiecesActive(board, false);

        int direction = piece.color ? 1 : -1;


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
        int fieldID = BoardUtil.StringToIndex(fieldName);
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
                board.doMove(5 + i, m1, m2, true, true, false);
            }
        }
        setPiecesActive(board, true);
    }

    private bool isCorrectPiece()
    {
        Player player = board.currentGame.players[board.currentGame.currentPlayer];
        int fieldID = BoardUtil.StringToIndex(m1);
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
        int fieldID = BoardUtil.StringToIndex(m2);
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
            if (BoardUtil.IndexToString(pm.x).Equals(m2)) {
            return pm.y; }
        }
        return -1;
    }



    
    
    private void setPiecesActive(Board board, bool active) {
        foreach (Piece p in board.whitePieces)
        {
            Field field = board.fields[p.position];
            field.pieceImage.gameObject.SetActive(active);
            field.refreshImage();
        }
        foreach (Piece p in board.blackPieces)
        {
            Field field = board.fields[p.position];
            field.pieceImage.gameObject.SetActive(active);
            field.refreshImage();
        }
    }
    private void refreshTurn(bool moveDone)
    {
        
        if(moveDone){
            board.fields[selectedField].selectedPieceImage.gameObject.SetActive(false);
        }

        m1 = "";
        m2 = "";
        board.drawOnBoard.refreshPossibles(true);

    }



    //  ***********
    //  FIELDCOLORS
    //  ***********
    public void SetSelectedField(int id)
    {
        board.fields[selectedField].selectedPieceImage.gameObject.SetActive(false);
        selectedField = id;
        board.fields[id].selectedPieceImage.gameObject.SetActive(true);
    }
    public bool CheckPossibleField(string field)
    {
        int id = BoardUtil.StringToIndex(field);
        foreach (Vector2Int pm in possibleMoves)
        {
            if (id == pm.x) {return true;}
        }
        return false;
    }
    public bool CheckOwnPiece(string field)
    {
        int id = BoardUtil.StringToIndex(field);
        if(board.fields[id].piece != null && board.fields[id].piece.color == board.currentGame.players[board.currentGame.currentPlayer].color)
        {
            return true;
        }
        return false;
    }
}
