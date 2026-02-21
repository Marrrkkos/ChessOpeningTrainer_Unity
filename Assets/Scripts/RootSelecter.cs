using Unity.VisualScripting;
using UnityEngine;

public class RootSelecter : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject Opening;
    public GameObject GameSense;
    public GameObject GameView;
    [Header("Loaders")]

    public OpeningLoader openingLoader;

    public void OnEnable()
    {
        string selected = GameManager.instance.selectedMode;
        Debug.Log(selected);
        if(selected == "GameSense")
        {
            GameSense.SetActive(true);
        }else if( selected == "Opening")
        {
            Opening.SetActive(true);
            openingLoader.LoadOpening();
        }else if(selected == "GameView")
        {
            GameView.SetActive(true);
        }
    }
    public void OnDisable()
    {
        Opening.SetActive(false); 
        GameSense.SetActive(false);
        GameView.SetActive(false);
    }

    
}