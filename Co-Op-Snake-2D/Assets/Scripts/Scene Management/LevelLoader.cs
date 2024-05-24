using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    public string buttonType;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        switch (buttonType)
        {
            case "Level":
                SceneManagerUtility.LoadScene(transform.GetSiblingIndex() + 1);
                break;
            case "Back":
                SceneManagerUtility.LoadMainMenu();
                break;
            default:
                Debug.LogWarning("Unknown button type: " + buttonType);
                break;
        }
    }
}
