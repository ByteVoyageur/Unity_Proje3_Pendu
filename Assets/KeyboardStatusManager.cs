using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardStatusManager : MonoBehaviour
{
    private VisualElement keyboardContainer;

    void OnEnable()
    {
        // get UIDocument
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No UIDocument component found!");
            return;
        }

        // get rootVisualElement
        VisualElement root = uiDocument.rootVisualElement;
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        // parcourir each button
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button)
            {
                button.clicked += () => UpdateButtonStatus(button);
            }
        }
    }

    private void UpdateButtonStatus(Button button)
    {
        // update button style
        button.AddToClassList("used-button");
        button.text = "X";
    }
}