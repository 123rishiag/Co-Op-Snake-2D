using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    // References to UI elements within the lobby.
    public Button playButton;
    public Button quitButton;
    public Button muteButton;
    public GameObject levelSelectorGameObject;

    private void Awake()
    {
        // Add listeners to buttons for corresponding methods.
        playButton.onClick.AddListener(PlayGame); // Listener for the Play button.
        quitButton.onClick.AddListener(QuitGame); // Listener for the Quit button.
        muteButton.onClick.AddListener(MuteGame); // Listener for the Mute button.
    }

    // Handles the mute button functionality.
    private void MuteGame()
    {
        // Get the TextMeshPro component from the mute button's children to change the display text.
        TextMeshProUGUI muteButtonText = muteButton.GetComponentInChildren<TextMeshProUGUI>();

        // Toggle the mute state and update the button's text accordingly.
        if (muteButtonText.text == "Mute: On")
        {
            muteButtonText.text = "Mute: Off"; // If currently muted, set to unmute.
        }
        else
        {
            muteButtonText.text = "Mute: On"; // If currently unmuted, set to mute.
        }
    }

    // Handles the play button functionality.
    private void PlayGame()
    {

        // Activate the lobby game object which can be a menu or a game scene.
        levelSelectorGameObject.SetActive(true);
    }

    // Handles the quit button functionality.
    private void QuitGame()
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
