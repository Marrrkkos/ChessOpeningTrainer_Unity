using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{
    public Board board;
    public BoardScaler boardScaler;
    public Image animationDummy; // Ziehe das neue Image hier in den Inspector
    public Transform animationLayer;
    public float duration = 0.4f;

    public void DoAnimation(Piece piece, int pos1, int pos2, int[] refreshIDs)
    {
        Image ghost = Instantiate(animationDummy, animationLayer);
        StartCoroutine(MoveGhostImage(pos1, pos2, piece, refreshIDs, ghost));
    }

    IEnumerator MoveGhostImage(int pos1, int pos2, Piece piece, int[] refreshIDs, Image ghost)
    {
        // 1. Vorbereitung: Ghost-Image konfigurieren
        Sprite pieceSprite = piece.color ? board.pieceSet.whitePieces[piece.id] : board.pieceSet.blackPieces[piece.id];
        ghost.sprite = pieceSprite;
        
        // Start- und Zielpositionen
        Vector3 startPos = board.fields[pos1].pieceImage.rectTransform.position;
        Vector3 endPos = board.fields[pos2].pieceImage.rectTransform.position;

        // Startfeld sofort visuell leeren
        board.fields[pos1].pieceImage.gameObject.SetActive(false);
        
        // Ghost aktivieren und auf Start setzen
        ghost.rectTransform.position = startPos;
        ghost.gameObject.SetActive(true);

        // 2. Die eigentliche Bewegung
        float elapsed = 0;
        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            ghost.rectTransform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetFinalPieces(pos1, pos2, piece, refreshIDs);         // Ziel-Bild anzeigen & Logik updaten
        Destroy(ghost.gameObject);
    }

    private void SetFinalPieces(int pos1, int pos2, Piece piece, int[] refreshIDs)
    {
        // Logik in der Datenstruktur updaten
        foreach(int id in refreshIDs)
        {
            board.fields[id].refreshImage();
        }

        // Visuelles Update des Zielfeldes
        board.fields[pos2].pieceImage.sprite = piece.color ? board.pieceSet.whitePieces[piece.id] : board.pieceSet.blackPieces[piece.id];
        board.fields[pos2].pieceImage.gameObject.SetActive(true);
    }
}

