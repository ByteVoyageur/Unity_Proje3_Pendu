using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardStatusManager : MonoBehaviour
{
    private VisualElement keyboardContainer;

    void OnEnable()
    {
        Debug.Log("KeyboardStatusManager OnEnable called");

        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No UIDocument component found!");
            return;
        }

        VisualElement root = uiDocument.rootVisualElement;
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        if (keyboardContainer == null)
        {
            Debug.LogError("No keyboard container found!");
            return;
        }

        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button") // Exclude the Next button
            {
                button.clicked += () =>
                {
                    Debug.Log($"Button {button.text} clicked");
                    UpdateButtonStatus(button);
                };
            }
        }
    }

    private void UpdateButtonStatus(Button button)
    {
        if (button.ClassListContains("used-button"))
        {
            Debug.Log($"Button {button.text} already updated");
            return;
        }

        Debug.Log($"Updating status for button {button.text}");
        button.AddToClassList("used-button");
        button.text = "X";
        Debug.Log($"Button {button.text} updated with class used-button");
    }

    public void ResetKeyboardStatus()
    {
        if (keyboardContainer == null)
        {
            Debug.LogError("No keyboard container found!");
            return;
        }

        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.ClassListContains("used-button"))
            {
                button.RemoveFromClassList("used-button");
                button.text = button.name.ToUpper();
            }
        }

        Debug.Log("Keyboard status reset successfully!");
    }

    public void DisableKeyboard()
    {
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button")
            {
                button.SetEnabled(false);
            }
        }
    }
}
