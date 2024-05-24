using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button muteButton;
    public GameObject levelSelectorGameObject;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        muteButton.onClick.AddListener(MuteGame);
    }

    private void MuteGame()
    {
        TextMeshProUGUI muteButtonText = muteButton.GetComponentInChildren<TextMeshProUGUI>();
        bool isMute = muteButtonText.text == "Mute: On";
        muteButtonText.text = isMute ? "Mute: Off" : "Mute: On";
        SoundManager.Instance.MuteGame();
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
    }

    private void PlayGame()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        levelSelectorGameObject.SetActive(true);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
