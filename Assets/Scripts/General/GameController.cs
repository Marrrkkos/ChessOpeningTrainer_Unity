using UnityEngine;


    
public class GameController : MonoBehaviour
{
    [Header("OpeningController")]
    public OpeningController openingController;
    
    [Header("OpeningController")]
    public StockFishController stockFishController;
    public bool stockFishActive = false;
    [Header("OpeningController")]
    public OpeningDataBaseController openingDataBaseController;
    public bool openingDataBaseActive = false;
    [Header("OpeningController")]
    public OpeningTrainingController openingTrainingController;
    public bool openingTrainingActive = false;


    public void OnMoveDone(Opening opening, Game currentGame)
    {
         if(openingTrainingController != null && openingTrainingActive)
            {

                if(opening.color != currentGame.players[currentGame.currentPlayer].color){
                    openingTrainingController.ManageNext();

                    openingController.DrawOpeningArrows();  // For tests
                }else{openingController.DrawOpeningArrows();}
                return;
            }
        if(opening.name != "")
        {
            openingController.DrawOpeningArrows();
        }
        if (stockFishController != null && stockFishActive)
        {
            stockFishController.DrawStockFishArrows(currentGame.playedMoves);
        }
        if (openingDataBaseController != null && openingDataBaseActive)
        {
            openingDataBaseController.GetOpeningMoves(currentGame.playedMoves);
        }
    }
}