using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button replayButton;
    public Button mainMenuButton;
    public GameObject players;
    public GameObject gameOverPrefab;
    public TextMeshProUGUI winnerText;

    private SnakeController[] snakeControllers;

    private void Awake()
    {
        snakeControllers = players.GetComponentsInChildren<SnakeController>();
        replayButton.onClick.AddListener(ReplayLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
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
        int highestScore = 0;
        string winner = "Draw!!";
        foreach (var snakeController in snakeControllers)
        {
            int score = snakeController.GetScore();
            if (score > highestScore)
            {
                highestScore = score;
                winner = LayerMask.LayerToName(snakeController.gameObject.layer) + " Wins!!";
            }
        }
        winnerText.text = winner;
    }

    public void GameOver()
    {
        SoundManager.Instance.PlayEffect(SoundType.LevelOver);
        gameOverPrefab.SetActive(true);
    }
}
