using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game
{
    public Player[] players { get; } = new Player[2];
    public string description;
    public int currentPlayer = 0;

    public int result = 0;

    public struct SimpleMove { 
        public int from; 
        public int to;
        public int specialRule;
        public string san;
    }

    public List<Move> playedMoves = new List<Move>();

    public List<Move> movesMemory = new List<Move>();
    public List<SimpleMove> loadedGame = new List<SimpleMove>();
    public Game(Player[] players) {
        this.players = players;
    }

}
