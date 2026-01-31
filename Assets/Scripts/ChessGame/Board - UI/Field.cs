using UnityEngine;
using UnityEngine.UI;
public class Field : MonoBehaviour
{
    public Board board;
    public Text fieldName;
    public Piece piece;
    public Image pieceImage;
    public Image possibleImage;
    public Image overLayImage;
    public void Awake() { 
        board = GetComponentInParent<Board>();
    }

    public void setPiece(Piece piece, bool refreshGUI) {

        this.piece = piece;
        if (refreshGUI)
        {
            if (piece != null)
            {
                pieceImage.gameObject.SetActive(true);
                if (piece.color)
                {
                    pieceImage.sprite = board.pieceSet.whitePieces[piece.id];
                }
                else
                {
                    pieceImage.sprite = board.pieceSet.blackPieces[piece.id];
                }
            }
            else
            {
                pieceImage.gameObject.SetActive(false);
            }
        }
    }
    public void setPieceImage(int i, bool color) {
        if (i == -1) {

            pieceImage.gameObject.SetActive(false);
        }
        else
        {
            pieceImage.gameObject.SetActive(true);
            if (color)
            {
                pieceImage.sprite = board.pieceSet.whitePieces[i];
            }
            else
            {
                pieceImage.sprite =board.pieceSet.blackPieces[i];
            }
        }
    }
    public void refreshImage(int i) {
        if (piece != null)
        {
            if (piece.color)
            {
                pieceImage.sprite = board.pieceSet.whitePieces[board.fields[i].piece.id];
            }
            else
            {
                pieceImage.sprite = board.pieceSet.blackPieces[board.fields[i].piece.id];
            }
        }
    }
}
