using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneAName = "MainScene";
    [SerializeField] private string sceneBName = "ChessBoardScene";

    private Scene sceneA, sceneB;

    async void Start()
    {
        // Beide Szenen additiv laden
        await SceneManager.LoadSceneAsync(sceneAName, LoadSceneMode.Additive);
        await SceneManager.LoadSceneAsync(sceneBName, LoadSceneMode.Additive);

        sceneA = SceneManager.GetSceneByName(sceneAName);
        sceneB = SceneManager.GetSceneByName(sceneBName);

        // Startzustand: Szene A aktiv, Szene B versteckt
        SetSceneState(sceneA, true);
        SetSceneState(sceneB, false);
    }

    public void Switch()
    {
        // Einfacher Toggle: Wenn A aktiv ist, schalte zu B
        bool isAActive = sceneA.GetRootGameObjects()[0].activeSelf;
        
        SetSceneState(sceneA, !isAActive);
        SetSceneState(sceneB, isAActive);
    }

    private void SetSceneState(Scene scene, bool state)
    {
        // Der Trick: Alle Root-Objekte der Szene deaktivieren/aktivieren
        // Das pausiert Scripte, Kameras und Rendering dieser Szene komplett
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            obj.SetActive(state);
        }

        if (state) SceneManager.SetActiveScene(scene);
    }
}
