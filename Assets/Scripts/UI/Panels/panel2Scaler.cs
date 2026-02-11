using UnityEngine;

public class panel2Scaler : MonoBehaviour
{
    public BoardScaler mainBoardScaler;
    public void scalePanel2(float panelWidth){
        mainBoardScaler.scaleBoard(panelWidth);

    }

}