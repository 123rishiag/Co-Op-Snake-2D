using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseController : MonoBehaviour
{
    public Button resumeButton;
    public Button mainMenuButton;
    public GameObject gamePausePrefab;
    public TextMeshProUGUI pauseText;

    private void Awake()
    {
        // Add listeners to buttons for corresponding methods.
        resumeButton.onClick.AddListener(ResumeLevel); // Listener for the Resume button.
        mainMenuButton.onClick.AddListener(GoToMainMenu); // Listener for the Main Menu button.
    }
    private void ResumeLevel()
    {
        // Activate the GamePause prefab
        gamePausePrefab.SetActive(false);
        Time.timeScale = 1f;
    }
    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManagerUtility.LoadMainMenu();
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        // Activate the GamePause prefab
        gamePausePrefab.SetActive(true);
    }
}
