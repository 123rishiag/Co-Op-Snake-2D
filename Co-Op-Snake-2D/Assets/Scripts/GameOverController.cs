using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button replayButton;
    public Button mainMenuButton;
    public GameObject players; // Reference to the parent object containing all snake players
    public GameObject gameOverPrefab;
    public TextMeshProUGUI winnerText;

    private SnakeController[] snakeControllers; // Array to hold all snake controllers

    private void Awake()
    {
        // Initialize the array with all SnakeController components found in the players object
        snakeControllers = players.GetComponentsInChildren<SnakeController>();
        // Add listeners to buttons for corresponding methods.
        replayButton.onClick.AddListener(ReplayLevel); // Listener for the Replay button.
        mainMenuButton.onClick.AddListener(GoToMainMenu); // Listener for the Main Menu button.
    }
    private void ReplayLevel()
    {
        SceneManagerUtility.ReloadCurrentScene();
    }
    private void GoToMainMenu()
    {
        SceneManagerUtility.LoadMainMenu();
    }
    public void UpdateWinner()
    {
        int currentScore = 0;
        foreach (var snakeController in snakeControllers)
        {
                if(currentScore < snakeController.GetScore())
                {
                    currentScore = snakeController.GetScore();
                    winnerText.text = LayerMask.LayerToName(snakeController.gameObject.layer) + " Wins!!";
                }
                else if(currentScore == snakeController.GetScore())
                {
                    winnerText.text = "Draw!!";
                }
        }
    }

    public void GameOver()
    {
        SoundManager.Instance.PlayEffect(SoundType.LevelOver);
        // If no active snakes are found, activate the GameOver prefab
        gameOverPrefab.SetActive(true);
    }
}
