interface IOpeningTrainer
{
    public void InitTraining(Opening opening, int depth, Board board, BoardScaler boardScaler);
    public void ManageNext();
    public void ResetTraining();

}