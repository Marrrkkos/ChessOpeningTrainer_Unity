using System;

public interface IPlayer
{
    void RequestMove(Board currentBoard, Action<Move> onMoveDecided);

}