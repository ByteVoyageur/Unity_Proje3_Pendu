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

                // Create a temporary label to animate
                var tempLabel = new Label { text = currentWord[i].ToString() };
                tempLabel.style.position = Position.Absolute;
                tempLabel.style.fontSize = 72;
                tempLabel.style.color = new StyleColor(new Color(0.133f, 0.545f, 0.133f));

                var centerX = (Screen.width - tempLabel.worldBound.width) / 2;
                var centerY = (Screen.height - tempLabel.worldBound.height) / 3;
                tempLabel.style.left = centerX;
                tempLabel.style.top = centerY;

                wordLabel.parent.Add(tempLabel);

                float finalXPosition = wordLabel.worldBound.xMin + i * 20; // Calculate final position
                float finalYPosition = wordLabel.worldBound.yMin;

                // Animation sequence with increased duration
                Sequence animationSequence = DOTween.Sequence();
                animationSequence.Append(DOTween.To(() => 1f, x => tempLabel.style.scale = new Scale(new Vector3(x, x, 1)), 1f, 2f)); // Increased scale up duration to 2 seconds
                animationSequence.Append(DOTween.To(() => 1f, x => 
                {
                    tempLabel.style.scale = new Scale(new Vector3(x, x, 1));
                    tempLabel.style.left = finalXPosition - tempLabel.worldBound.width * (1 - x) / 2;
                    tempLabel.style.top = finalYPosition - tempLabel.worldBound.height * (1 - x) / 2;
                }, 0.1f, 2f)); // Increased scale down duration to 2 seconds
                animationSequence.Append(tempLabel.DOFade(0, 1f)); // Increased fade out duration to 1 second
                animationSequence.OnComplete(() => tempLabel.RemoveFromHierarchy());
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
