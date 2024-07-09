using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class NextButtonHandler : MonoBehaviour
{
    private WordGenerator wordGenerator;
    private WordMatcher wordMatcher;
    private VisualElement keyboardContainer;

    void Start()
    {
        wordGenerator = GetComponent<WordGenerator>();
        wordMatcher = GetComponent<WordMatcher>();
        UIDocument uiDocument = GetComponent<UIDocument>();
        keyboardContainer = uiDocument.rootVisualElement.Q<VisualElement>("KeyboardButtons");
    }

    public void OnNextButtonClick()
    {
        Debug.Log("Next button clicked!");

        if (keyboardContainer == null)
        {
            UIDocument uiDocument = GetComponent<UIDocument>();
            keyboardContainer = uiDocument.rootVisualElement.Q<VisualElement>("KeyboardButtons");
        }

        // Reset keyboard buttons state
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button")
            {
                button.text = button.name.ToUpper();
                button.RemoveFromClassList("used-button");
            }
        }

        // Clear previous word state
        wordMatcher.Initialize(string.Empty);

        // Request a new word
        StartCoroutine(wordGenerator.GetRandomWordFromAPI());
    }
}