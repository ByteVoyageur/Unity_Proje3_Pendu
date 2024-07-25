using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class WordMatcher : MonoBehaviour
{
    private string currentWord;
    private string normalizedWord;
    private Label wordLabel;
    private VisualElement keyboardContainer;
    private bool[] matchedLetters;
    private bool inputDisabled = false;

    public event Action<bool> OnWordMatched;
    public event Action OnNewWordInitialized;

    private MatchResultManager matchResultManager;
    private KeyboardStatusManager keyboardStatusManager;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        wordLabel = root.Q<Label>("WordLabel");
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        matchResultManager = GetComponent<MatchResultManager>();
        keyboardStatusManager = GetComponent<KeyboardStatusManager>();

        keyboardStatusManager.OnButtonUsed += HandleButtonUsed;
    }

    public void Initialize(string word, string normalized)
    {
        currentWord = word;
        normalizedWord = normalized;
        matchedLetters = new bool[currentWord.Length];
        inputDisabled = false;

        OnNewWordInitialized?.Invoke();
        matchResultManager.SetMaxAttempts(currentWord.Length);
        UpdateWordLabel();
    }

    public void Initialize(string word)
    {
        Initialize(word, WordNormalizer.NormalizeString(word));
    }

    public bool OnButtonClick(char inputLetter)
    {
        if (inputDisabled)
            return false;

        bool matched = false;

        for (int i = 0; i < normalizedWord.Length; i++)
        {
            if (normalizedWord[i] == inputLetter)
            {
                matchedLetters[i] = true;
                matched = true;

                // Get the target label position
                float labelX = wordLabel.worldBound.xMin + i * 20; // Adjust according to the label position
                float labelY = wordLabel.worldBound.yMin;

                // Play animation and update label
                LetterAnimation.UpdateLabelWithAnimation(wordLabel.parent, currentWord[i].ToString(), new Color(0.133f, 0.545f, 0.133f), labelX, labelY);
            }
        }

        UpdateWordLabel();

        bool allMatched = !Array.Exists(matchedLetters, matched => matched == false);
        OnWordMatched?.Invoke(allMatched);

        if (!matched)
        {
            matchResultManager.UpdateFailedAttempts();
        }

        return matched;
    }

    public void UpdateWordLabel(bool showAllLetters = false)
    {
        string richText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (matchedLetters[i])
            {
                richText += $"<color=#228B22>{currentWord[i]}</color>"; // deep green
            }
            else if (showAllLetters)
            {
                richText += $"<color=#FF0000>{currentWord[i]}</color>";
            }
            else
            {
                richText += "_";
            }
        }
        wordLabel.text = richText;
    }

    private void HandleButtonUsed(Button button)
    {
        button.SetEnabled(false);
    }
}
