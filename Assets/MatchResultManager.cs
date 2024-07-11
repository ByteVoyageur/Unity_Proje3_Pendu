using UnityEngine;
using UnityEngine.UIElements;

public class MatchResultManager : MonoBehaviour
{
    private VisualElement resultContainer;
    private Label successLabel;
    private Label failedLabel;
    private VisualElement keyboardContainer;
    private WordMatcher wordMatcher;
    private Button nextButton;

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
        nextButton = root.Q<Button>("next-button");

        wordMatcher = GetComponent<WordMatcher>();
        wordMatcher.OnWordMatched += HandleWordMatched;
        wordMatcher.OnNewWordInitialized += ResetAttemptsAndResults;

        resultContainer.style.display = DisplayStyle.Flex;
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
            isGameRunning = false;
            EnableNextButton(); // Enable Next button on success
        }
        else if (currentAttempts >= maxAttemptsFail)
        {
            ShowFailure();
            DisableKeyboard();
            isGameRunning = false;
            EnableNextButton(); // Enable Next button on failure
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
        nextButton.SetEnabled(true);
    }
}