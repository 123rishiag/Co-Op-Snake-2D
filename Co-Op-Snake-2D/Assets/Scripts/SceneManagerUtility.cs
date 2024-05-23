using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    // Handles loading a specific scene by its index.
    public static void LoadScene(int sceneIndex)
    {
        // Load the scene with the provided index.
        SceneManager.LoadScene(sceneIndex);
    }

    // Reloads the current scene.
    public static void ReloadCurrentScene()
    {
        // Retrieve the index of the currently active scene.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Use LoadScene to reload the current scene using its index.
        LoadScene(currentSceneIndex);
    }

    // Loads the main menu scene, which is at index 0.
    public static void LoadMainMenu()
    {
        // Load the main menu scene using index 0.
        SceneManager.LoadScene(0);
    }
    // Quits the game application.
    public static void QuitGame()
    {
        // Check if the application is running in the Unity Editor.
        #if UNITY_EDITOR
        {
            // Stop playing the scene in the Unity Editor.
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #else
        Application.Quit();
        #endif
    }
}
