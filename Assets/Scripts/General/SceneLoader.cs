using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void SwitchScene()
    {
        GameManager.instance.sceneSwitcher.Switch();
    }

}