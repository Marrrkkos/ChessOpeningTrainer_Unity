using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneAName = "MainScene";
    [SerializeField] private string sceneBName = "ChessBoardScene";

    private Scene sceneA, sceneB;

    async void Start()
    {
        await SceneManager.LoadSceneAsync(sceneAName, LoadSceneMode.Additive);
        await SceneManager.LoadSceneAsync(sceneBName, LoadSceneMode.Additive);

        sceneA = SceneManager.GetSceneByName(sceneAName);
        sceneB = SceneManager.GetSceneByName(sceneBName);

        SetSceneState(sceneA, true);
        SetSceneState(sceneB, false);
    }

    public void Switch()
    {
        bool isAActive = sceneA.GetRootGameObjects()[0].activeSelf;
        
        SetSceneState(sceneA, !isAActive);
        SetSceneState(sceneB, isAActive);
    }

    private void SetSceneState(Scene scene, bool state)
    {
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            obj.SetActive(state);
        }

        if (state) SceneManager.SetActiveScene(scene);
    }
}
