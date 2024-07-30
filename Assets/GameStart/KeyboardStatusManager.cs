using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
public class KeyboardStatusManager : MonoBehaviour
{
    private VisualElement keyboardContainer;

    public event Action<Button> OnButtonUsed;

    void OnEnable()
    {
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

    private void UpdateButtonStatus(Button button)
    {
        if (button.ClassListContains("used-button"))
        {
            Debug.Log($"Button {button.text} already updated");
            return;
        }

        button.AddToClassList("used-button");
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
    }

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
