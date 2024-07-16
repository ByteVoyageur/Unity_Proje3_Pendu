using UnityEngine;
using UnityEngine.UIElements;

public class MatchResultManager : MonoBehaviour
{
    private VisualElement resultContainer;
    private Label successLabel;
    private Label failedLabel;
    private VisualElement keyboardContainer;
    private WordMatcher wordMatcher;
    private GameStatsManager gameStatsManager;
    private HangmanAnimator hangmanAnimator;
    private KeyboardStatusManager keyboardStatusManager;

    private int maxAttemptsSuccess;
    private int maxAttemptsFail = 10;
    private int currentAttempts = 0;
    public bool isGameRunning = false;

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

        gameStatsManager = GetComponent<GameStatsManager>();
        hangmanAnimator = GetComponent<HangmanAnimator>(); // Initialize HangmanAnimator
        keyboardStatusManager = GetComponent<KeyboardStatusManager>(); // Initialize KeyboardStatusManager
    }

    private void OnDisable()
    {
        wordMatcher.OnWordMatched -= HandleWordMatched;
        wordMatcher.OnNewWordInitialized -= ResetAttemptsAndResults;
    }

    private void HandleWordMatched(bool allMatched)
    {
        if (allMatched)
        {
            ShowSuccess();
            isGameRunning = false;
            gameStatsManager.IncrementWinCount();
            keyboardStatusManager.DisableKeyboard(); // Disable the keyboard after success
            EnableNextButton();
        }
    }

    public void UpdateFailedAttempts()
    {
        if (isGameRunning)
        {
            currentAttempts++;
            hangmanAnimator.ShowNextPart(); // Show next hangman part

            if (currentAttempts >= maxAttemptsFail)
            {
                ShowFailure();
                wordMatcher.UpdateWordLabel(true); // Show all letters in red for unmatched ones
                DisableKeyboard();
                isGameRunning = false;
                gameStatsManager.IncrementLoseCount();
                EnableNextButton();
            }
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
        isGameRunning = true;

        hangmanAnimator.ResetAnimation(); // Reset hangman animation
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
