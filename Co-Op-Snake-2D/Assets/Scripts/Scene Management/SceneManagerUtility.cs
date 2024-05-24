using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    public static void LoadScene(int sceneIndex)
    {
        SoundManager.Instance.PlayEffect(SoundType.LevelStart);
        SceneManager.LoadScene(sceneIndex);
    }

    public static void ReloadCurrentScene()
    {
        SoundManager.Instance.PlayEffect(SoundType.LevelStart);
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadMainMenu()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        SceneManager.LoadScene(0);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
