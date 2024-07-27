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
        wordLabel.Clear();  
        for (int i = 0; i < currentWord.Length; i++)
        {
            var letterElement = new Label(currentWord[i].ToString());
            
            if (matchedLetters[i])
            {
                letterElement.AddToClassList("matched-letter");
                letterElement.style.color = new Color(0.13f, 0.55f, 0.13f); 
            }
            else if (showAllLetters)
            {
                letterElement.style.color = new Color(1.0f, 0.0f, 0.0f);
            }
            else
            {
                letterElement.text = " "; 
            }

            wordLabel.Add(letterElement); 
        }

        // 在所有子元素正确创建后，应用动画
        AnimationHelper.ApplyMatchedLetterAnimations(wordLabel, this);
    }

    private void HandleButtonUsed(Button button)
    {
        button.SetEnabled(false);
    }
}