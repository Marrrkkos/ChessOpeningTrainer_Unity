using UnityEngine;

public class RootSelecter : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject OpeningTop;
    public GameObject OpeningBot;

    public GameObject GameSenseTop;
    public GameObject GameSenseBot;

    [Header("Loaders")]

    public OpeningLoader openingLoader;

    public void OnEnable()
    {
        string selected = GameManager.instance.selectedMode;
        if(selected == "GameSense")
        {
            GameSenseTop.SetActive(true);
            GameSenseBot.SetActive(true);
        }else if( selected == "Opening")
        {
            OpeningTop.SetActive(true);
            OpeningBot.SetActive(true);
            openingLoader.LoadOpening();
        }
    }
    public void OnDisable()
    {
        OpeningTop.SetActive(false);
        OpeningBot.SetActive(false);
        GameSenseTop.SetActive(false);
        GameSenseBot.SetActive(false);
    }
}