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
        resultContainer = root.Q<VisualElement>("ResultLabel");
        successLabel = root.Q<Label>("SuccessLabel");
        failedLabel = root.Q<Label>("FailedLabel");
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        // Get the WordMatcher component
        wordMatcher = GetComponent<WordMatcher>();

        // Subscribe to the word match event
        wordMatcher.OnWordMatched += HandleWordMatched;
        wordMatcher.OnNewWordInitialized += ResetAttemptsAndResults; // Subscribe to new event

        // Show the result container initially
        resultContainer.style.display = DisplayStyle.Flex; // Ensure the result container is visible initially
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
        successLabel.style.display = DisplayStyle.Flex;
        failedLabel.style.display = DisplayStyle.None;
        successLabel.style.color = Color.green;
    }

    private void ShowFailure()
    {
        successLabel.style.display = DisplayStyle.None;
        failedLabel.style.display = DisplayStyle.Flex;
        failedLabel.style.color = Color.red;
    }

    private void ResetAttemptsAndResults()
    {
        currentAttempts = 0;
        EnableKeyboard();
        // Ensure both success and fail labels are hidden initially
        successLabel.style.display = DisplayStyle.None;
        failedLabel.style.display = DisplayStyle.None;
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
