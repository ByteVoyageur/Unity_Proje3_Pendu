using System;
using UnityEngine;
using UnityEngine.UIElements;

// This class handles user input and matches it against the current word
public class WordMatcher : MonoBehaviour
{
    private string currentWord;
    private string normalizedWord;
    private Label wordLabel;
    private VisualElement keyboardContainer;
    private bool[] matchedLetters; // Array to keep track of matched letters
    private bool inputDisabled = false; // Flag to disable input after max attempts

    public event Action<bool> OnWordMatched; // Event to notify word match result
    public event Action OnNewWordInitialized; // Event to notify new word initialization

    private MatchResultManager matchResultManager;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        wordLabel = root.Q<Label>("WordLabel");
        keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        matchResultManager = GetComponent<MatchResultManager>();
    }

    // Initialize the current word and its normalized version
    public void Initialize(string word, string normalized)
    {
        currentWord = word; // Keep the original word for display
        normalizedWord = normalized; // Use the normalized word for matching
        matchedLetters = new bool[currentWord.Length]; // Initialize matchedLetters array
        inputDisabled = false; // Enable input on new word initialization

        // Notify new word initialization
        OnNewWordInitialized?.Invoke();

        // Set max attempts in MatchResultManager
        matchResultManager.SetMaxAttempts(currentWord.Length);

        // Update the word label
        UpdateWordLabel();
    }

    // Handle button click event
    public void OnButtonClick(char inputLetter)
    {
        if (inputDisabled)
            return;

        bool matched = false;

        for (int i = 0; i < normalizedWord.Length; i++)
        {
            if (normalizedWord[i] == inputLetter)
            {
                matchedLetters[i] = true; // Record that the letter at the current position has been matched
                matched = true;
            }
        }

        // Update the WordLabel with the new rich text
        UpdateWordLabel();

        // Notify the result
        bool allMatched = !Array.Exists(matchedLetters, matched => matched == false);
        OnWordMatched?.Invoke(allMatched);

        // If no match found, update failed attempts
        if (!matched)
        {
            matchResultManager.UpdateFailedAttempts();
        }
    }

    // Update the WordLabel with the current state of matched letters
    public void UpdateWordLabel(bool showAllLetters = false)
    {
        string richText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (matchedLetters[i])
            {
                richText += $"<color=#90EE90>{currentWord[i]}</color>";
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

    // Existing Initialize method for backward compatibility
    public void Initialize(string word)
    {
        Initialize(word, WordNormalizer.NormalizeString(word)); // Call the new Initialize method with normalized word
    }
}
