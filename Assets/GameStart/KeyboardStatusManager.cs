using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
    /// <summary>
    /// Manages the status of the on-screen keyboard, including enabling/disabling buttons,
    /// updating button statuses, and resetting the keyboard state.
    /// </summary>
public class KeyboardStatusManager : MonoBehaviour
{
    private VisualElement keyboardContainer;

    public event Action<Button> OnButtonUsed;

// Initializes the keyboard container and button event handlers.
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button" && button.name != "return-button")
            {
                button.clicked += () =>
                {
                    UpdateButtonStatus(button);
                    OnButtonUsed?.Invoke(button);
                };
            }
        }
    }

//Updates the status of a fiven button to mark it as used.
    private void UpdateButtonStatus(Button button)
    {
        if (button.ClassListContains("used-button"))
        {
            Debug.Log($"Button {button.text} already updated");
            return;
        }

        button.AddToClassList("used-button");
    }

// Resets the status of the keyboard, clearing the "used" status from all buttons.
    public void ResetKeyboardStatus()
    {
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.ClassListContains("used-button"))
            {
                button.RemoveFromClassList("used-button");
                button.text = button.name.ToUpper();
            }
        }
    }

//Disables all the buttons on the keyboard except "next-button" and "return-button".
    public void DisableKeyboard()
    {
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button" && button.name != "return-button")
            {
                button.SetEnabled(false);
            }
        }
    }

    public void EnableKeyboard()
    {
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button)
            {
                button.SetEnabled(true);
            }
        }
    }
}
}
