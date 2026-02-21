using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    
    public SureDelete sureDelete;
    public static MainSceneManager instance;

    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else {
            Destroy(gameObject);
        }
    
    }
}
