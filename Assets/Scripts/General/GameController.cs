using UnityEngine;


    
public class GameController : MonoBehaviour
{
    [Header("OpeningController (Optional)")]
    public OpeningController openingController;
    
    [Header("OpeningController (Optional)")]
    public StockFishController stockFishController;
    public bool stockFishActive = false;
    [Header("OpeningController (Optional)")]
    public OpeningDataBaseController openingDataBaseController;
    public bool openingDataBaseActive = false;
    [Header("OpeningController (Optional)")]
    public OpeningTrainingController openingTrainingController;
    public bool openingTrainingActive = false;
    public bool playerHasMoved = false;

    public void OnMoveDone(Opening opening, Game currentGame)
    {
         /*if(openingTrainingController != null && openingTrainingActive)
            {

                if(opening.color != currentGame.players[currentGame.currentPlayer].color){
                    openingTrainingController.ManageNext();

                    openingController.DrawOpeningArrows();  // For tests
                }else{openingController.DrawOpeningArrows();}
                return;
            }*/
        if(openingController != null && opening.name != "")
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