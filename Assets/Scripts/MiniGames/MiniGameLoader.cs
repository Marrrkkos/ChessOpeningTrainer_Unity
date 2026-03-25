using UnityEngine;

public class MiniGameLoader : MonoBehaviour
{
    public void LoadGameSense()
    {
        GameManager.instance.selectedMode = "GameSense";
        GameManager.instance.sceneSwitcher.Switch();
    }



}