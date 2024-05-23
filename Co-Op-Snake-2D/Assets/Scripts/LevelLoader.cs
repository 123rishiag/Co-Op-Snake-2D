using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    public string buttonType;

    private void Awake()
    {
        // Initializes the button component and adds an event listener to it.
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick); // Attach the OnClick method to the button's click event.
    }

    private void OnClick()
    {
        // Handles click events based on the type of button.
        if (buttonType == "Level")
        {
            // If the button type is "Level", handle level loading.
            int levelIndex = transform.GetSiblingIndex(); // Determine the level index based on the button's position in the UI hierarchy.
            SceneManagerUtility.LoadScene(levelIndex + 1);
        }
        else if (buttonType == "Back")
        {
            SceneManagerUtility.LoadMainMenu();
        }
    }
}
