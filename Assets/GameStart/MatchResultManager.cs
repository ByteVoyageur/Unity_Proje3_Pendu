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
    private SoundManager soundManager;

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
        hangmanAnimator = GetComponent<HangmanAnimator>();
        keyboardStatusManager = GetComponent<KeyboardStatusManager>();

        soundManager = FindObjectOfType<SoundManager>();
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
            keyboardStatusManager.DisableKeyboard();
            EnableNextButton();
            soundManager.PlayWinSound();
        }
    }

    public void UpdateFailedAttempts()
    {
        if (isGameRunning)
        {
            currentAttempts++;
            hangmanAnimator.ShowNextPart();

            if (currentAttempts >= maxAttemptsFail)
            {
                ShowFailure();
                wordMatcher.UpdateWordLabel(true);
                keyboardStatusManager.DisableKeyboard();
                isGameRunning = false;
                gameStatsManager.IncrementLoseCount();
                EnableNextButton();
                soundManager.PlayLoseSound();
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
        keyboardStatusManager.EnableKeyboard();
        successLabel.style.display = DisplayStyle.None;
        failedLabel.style.display = DisplayStyle.None;
        isGameRunning = true;
        hangmanAnimator.ResetAnimation();
    }

    public void SetMaxAttempts(int wordLength)
    {
        maxAttemptsSuccess = wordLength;
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
