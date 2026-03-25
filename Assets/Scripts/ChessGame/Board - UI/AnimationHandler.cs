using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    public float duration = 0.4f;

    public void DoAnimation(Piece piece, int pos1, int pos2, int[] refreshIDs)
    {
        Image ghost = Instantiate(board.fields[63].pieceImage, board.fields[63].transform);
        ghost.transform.position = board.fields[pos1].pieceImage.transform.position;
        StartCoroutine(MoveGhostImage(pos1, pos2, piece, refreshIDs, ghost));
    }

    IEnumerator MoveGhostImage(int pos1, int pos2, Piece piece, int[] refreshIDs, Image ghost)
    {
        Sprite pieceSprite = piece.color ? board.pieceSet.whitePieces[piece.id] : board.pieceSet.blackPieces[piece.id];
        ghost.sprite = pieceSprite;
        
        Vector3 startPos = board.fields[pos1].pieceImage.rectTransform.position;
        Vector3 endPos = board.fields[pos2].pieceImage.rectTransform.position;

        board.fields[pos1].pieceImage.gameObject.SetActive(false);
        
        ghost.rectTransform.position = startPos;
        ghost.gameObject.SetActive(true);

        float elapsed = 0;
        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            ghost.rectTransform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetFinalPieces(pos1, pos2, piece, refreshIDs);
        Destroy(ghost.gameObject);
    }

    private void SetFinalPieces(int pos1, int pos2, Piece piece, int[] refreshIDs)
    {
        foreach(int id in refreshIDs)
        {
            board.fields[id].refreshImage();
        }

        board.fields[pos2].pieceImage.sprite = piece.color ? board.pieceSet.whitePieces[piece.id] : board.pieceSet.blackPieces[piece.id];
        board.fields[pos2].pieceImage.gameObject.SetActive(true);
    }
}

