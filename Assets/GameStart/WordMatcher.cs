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

        // 只有匹配成功时才应用动画
        if (matched)
        {
            AnimationHelper.ApplyMatchedLetterAnimations(wordLabel, this);
        }

        return matched;
    }

    public void UpdateWordLabel(bool showAllLetters = false)
    {
        wordLabel.Clear();  // 清空现有的子元素

        // 设置 wordLabel 的布局为水平排列
        wordLabel.style.flexDirection = FlexDirection.Row;

        for (int i = 0; i < currentWord.Length; i++)
        {
            var letterElement = new Label();

            if (matchedLetters[i])
            {
                letterElement.text = currentWord[i].ToString(); 
                letterElement.AddToClassList("matched-letter");
                letterElement.style.color = new Color(0.13f, 0.55f, 0.13f); // 深绿色
            }
            else if (showAllLetters)
            {
                letterElement.text = currentWord[i].ToString(); 
                letterElement.style.color = new Color(1.0f, 0.0f, 0.0f); // 红色
            }
            else
            {
                // 仅在未匹配到且不展示所有字母时显示下划线
                letterElement.text = "_"; 
            }

            wordLabel.Add(letterElement); // 添加子元素到 wordLabel
        }
    }

    private void HandleButtonUsed(Button button)
    {
        button.SetEnabled(false);
    }
}