using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Components
    private SavesManager _savesManager;
    #endregion

    private void Start()
    {
        _savesManager = GameManager.GetInstance.GetSavesManager;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    // como la idea es hacerlo con eventos y no
    // podia quedar con 2 parametros hice 2 metodos
    public void ChangeLevelKeepStats(string sceneName)
    {
        _savesManager.DeleteCheckpoints(true);
        _savesManager.SaveStats();
        LoadScene(sceneName);
    }

    public void ChangeLevelDeleteStats(string sceneName)
    {
        _savesManager.DeleteCheckpoints(false);
        LoadScene(sceneName);
    }

    public void LoadGame()
    {
        LoadScene(_savesManager.GetCurrentLevel);
    }
}
