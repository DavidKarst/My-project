using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string recordScene = "Torben_Record_Card";
    [SerializeField] string puzzleScene = "Torbn_Puzzel";

    public void LoadRecord() => Load(recordScene);
    public void LoadPuzzle() => Load(puzzleScene);
    public void Reload() => Load(SceneManager.GetActiveScene().name);

    void Load(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

    }


}

