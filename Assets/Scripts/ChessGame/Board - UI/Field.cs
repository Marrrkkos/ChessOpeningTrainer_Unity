using UnityEngine;
using UnityEngine.UI;
public class Field : MonoBehaviour
{
    public Board board;
    public RawImage fieldImage;
    public Piece piece;
    public Image pieceImage;
    public Image possibleImage;
    public Image onPossibleHoverImage;
    public Image lastMoveImage;
    public Image selectedPieceImage;
    public void Awake() { 
        board = GetComponentInParent<Board>();
    }

    public void SetPiece(Piece piece, bool refreshGUI) {

        this.piece = piece;
        if (!refreshGUI)
        {
            return;
        }
        if (piece != null)
        {
            pieceImage.gameObject.SetActive(true);
            if (piece.color)
            {
                pieceImage.sprite = board.pieceSet.whitePieces[piece.id];
            }
            else
            {
                if(board == null)
                {
                    Debug.Log("Board");
                }
                if(board.pieceSet == null)
                {
                    Debug.Log("board.pieceSet");
                }
                if(board.pieceSet.blackPieces == null)
                {
                    Debug.Log("board.pieceSet.blackPieces");
                }
                pieceImage.sprite = board.pieceSet.blackPieces[piece.id];
            }
        }
        else
        {
            pieceImage.gameObject.SetActive(false);
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
    public void refreshImage() {
        if (piece != null)
        {
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
