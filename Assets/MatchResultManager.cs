using UnityEngine;
using UnityEngine.UIElements;

public class MatchResultManager : MonoBehaviour
{
    private VisualElement resultContainer;
    private Label successLabel;
    private Label failedLabel;
    private VisualElement keyboardContainer;
    private WordMatcher wordMatcher;
    private GameStatsManager gameStatsManager;  // Add reference to GameStatsManager

    private int maxAttemptsSuccess;  // Corrected variable name
    private int maxAttemptsFail = 10;
    private int currentAttempts = 0;
    public bool isGameRunning = false; // Track game running status

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        resultContainer = root.Q<VisualElement>("ResultLabel");
        successLabel = root.Q<Label>("SuccessLabel");
        failedLabel = root.Q<Label>("FailedLabel");
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");
        
        wordMatcher = GetComponent<WordMatcher>();
        wordMatcher.OnWordMatched += HandleWordMatched;
        wordMatcher.OnNewWordInitialized += ResetAttemptsAndResults;

        resultContainer.style.display = DisplayStyle.Flex;

        gameStatsManager = GetComponent<GameStatsManager>(); // Initialize GameStatsManager
    }

    private void OnDisable()
    {
        wordMatcher.OnWordMatched -= HandleWordMatched;
        wordMatcher.OnNewWordInitialized -= ResetAttemptsAndResults;
    }

    private void HandleWordMatched(bool isMatched)
    {
        currentAttempts++;

        if (isMatched)
        {
            ShowSuccess();
            isGameRunning = false; // End the game on success
            gameStatsManager.IncrementWinCount(); // Increment win count
            EnableNextButton();
        }
        else if (currentAttempts >= maxAttemptsFail)
        {
            ShowFailure();
            DisableKeyboard();
            isGameRunning = false; // End the game on failure
            gameStatsManager.IncrementLoseCount(); // Increment lose count
            EnableNextButton();
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
        successLabel.style.display = DisplayStyle.None;
        failedLabel.style.display = DisplayStyle.None;
        isGameRunning = true; // Start the game on new word initialization
    }

    public void SetMaxAttempts(int wordLength)
    {
        maxAttemptsSuccess = wordLength;
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

    private void EnableNextButton()
    {
        Button nextButton = keyboardContainer.Q<Button>("next-button");
        if (nextButton != null)
        {
            nextButton.SetEnabled(true);
        }
    }
}