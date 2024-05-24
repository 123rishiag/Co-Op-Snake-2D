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
        resumeButton.onClick.AddListener(ResumeLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void ResumeLevel()
    {
        SoundManager.Instance.PlayEffect(SoundType.LevelPause);
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
        SoundManager.Instance.PlayEffect(SoundType.LevelPause);
        Time.timeScale = 0f;
        gamePausePrefab.SetActive(true);
    }
}
