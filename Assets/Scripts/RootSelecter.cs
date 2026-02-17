using UnityEngine;

public class RootSelecter : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject Opening;
    public GameObject GameSense;
    [Header("Loaders")]

    public OpeningLoader openingLoader;

    public void OnEnable()
    {
        string selected = GameManager.instance.selectedMode;
        if(selected == "GameSense")
        {
            GameSense.SetActive(true);
        }else if( selected == "Opening")
        {
            Opening.SetActive(true);
            openingLoader.LoadOpening();
        }
    }
    public void OnDisable()
    {
        Opening.SetActive(false);
        GameSense.SetActive(false);
    }
}