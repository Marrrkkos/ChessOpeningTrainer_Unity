using System;
using UnityEngine;

[Serializable]
public class Move
{
    //ESSENTIALS
    public int from;
    public int to;
    public Piece movedPiece;
    public Piece capturedPiece;
    public int specialRule;
    public bool oldHasMoved;
    public bool check;
    public bool checkMate;
    public string san;

    //OPTIONAL
    public string description;
    public float time;
    public Move(int from, int to, Piece movedPiece, Piece capturedPiece, int specialRule, bool oldHasMoved, bool check, bool checkMate) {
        this.from = from;
        this.to = to;
        this.movedPiece = movedPiece;
        this.capturedPiece = capturedPiece;
        this.specialRule = specialRule;
        this.oldHasMoved = oldHasMoved;
        this.check = check;
        this.checkMate = checkMate;
    }


    public bool equals(Move other) { 
        return (from == other.from) && (to == other.to) && (movedPiece?.id == other.movedPiece?.id) && (capturedPiece?.id == other.capturedPiece?.id) && (specialRule == other.specialRule) && (oldHasMoved == other.oldHasMoved) && (check == other.check) && (checkMate == other.checkMate); 
    }

}
