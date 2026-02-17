 using UnityEngine;

[CreateAssetMenu(fileName = "PieceSet", menuName = "Chess/PieceSet")]
public class PieceSet : ScriptableObject
{
    public Sprite[] whitePieces;
    public Sprite[] blackPieces;
}
