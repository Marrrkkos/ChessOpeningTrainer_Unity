using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   
    public static void SwitchScene()
    {
        GameManager.instance.sceneSwitcher.Switch();
    }

}