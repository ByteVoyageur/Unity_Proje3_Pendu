using UnityEngine;
using UnityEngine.UIElements;

public class MatchResultManager : MonoBehaviour
{
    private VisualElement resultContainer;
    private Label successLabel;
    private Label failedLabel;
    private VisualElement keyboardContainer;
    private WordMatcher wordMatcher;

    private int maxAttemptsSuccess;
    private int maxAttemptsFail = 10;
    private int currentAttempts = 0;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Get the result container and labels
        resultContainer = root.Q<VisualElement>("ResultatLabel");
        successLabel = root.Q<Label>("SuccessLabel");
        failedLabel = root.Q<Label>("FaildLabel");
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        // Hide the result container initially
        resultContainer.style.display = DisplayStyle.None;

        // Get the WordMatcher component
        wordMatcher = GetComponent<WordMatcher>();

        // Subscribe to the word match event
        wordMatcher.OnWordMatched += HandleWordMatched;
        wordMatcher.OnNewWordInitialized += ResetAttemptsAndResults; // Subscribe to new event
    }

    private void OnDisable()
    {
        // Unsubscribe from the word match event
        wordMatcher.OnWordMatched -= HandleWordMatched;
        wordMatcher.OnNewWordInitialized -= ResetAttemptsAndResults; // Unsubscribe from new event
    }

    private void HandleWordMatched(bool isMatched)
    {
        currentAttempts++;

        if (isMatched)
        {
            ShowSuccess();
        }
        else if (currentAttempts >= maxAttemptsFail)
        {
            ShowFailure();
            DisableKeyboard();
        }
    }

    private void ShowSuccess()
    {
        resultContainer.style.display = DisplayStyle.Flex;
        successLabel.style.display = DisplayStyle.Flex;
        failedLabel.style.display = DisplayStyle.None;
        successLabel.style.color = Color.green;
    }

    private void ShowFailure()
    {
        resultContainer.style.display = DisplayStyle.Flex;
        successLabel.style.display = DisplayStyle.None;
        failedLabel.style.display = DisplayStyle.Flex;
        failedLabel.style.color = Color.red;
    }

    private void ResetAttemptsAndResults()
    {
        currentAttempts = 0;
        resultContainer.style.display = DisplayStyle.None;
        EnableKeyboard();
    }

    public void SetMaxAttempts(int wordLength)
    {
        maxAttemptsSuccess = wordLength; // Set max success attempts to the word length
    }

    private void DisableKeyboard()
    {
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button && button.name != "next-button")
            {
                button.SetEnabled(false);
            }
        }
    }

    private void EnableKeyboard()
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
