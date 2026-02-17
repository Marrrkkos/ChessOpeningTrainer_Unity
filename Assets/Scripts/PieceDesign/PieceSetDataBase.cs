using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PieceSetDatabase", menuName = "Chess/PieceSetDatabase")]
public class PieceSetDataBase : ScriptableObject
{
    public List<PieceSet> listOfSets;

    public PieceSet getPieceSet(int id) {
        return listOfSets[id];
    }
}
