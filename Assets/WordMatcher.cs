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
        Debug.Log($"Initialized with word: {currentWord}, normalized: {normalizedWord}");

        // Notify new word initialization
        OnNewWordInitialized?.Invoke();

        // Set max attempts in MatchResultManager
        matchResultManager.SetMaxAttempts(currentWord.Length);
    }

    // Handle button click event
    public void OnButtonClick(char inputLetter)
    {
        if (inputDisabled)
            return;

        Debug.Log($"Checking input letter: {inputLetter} against word: {normalizedWord}"); // Log the input letter and normalized word

        bool matched = false;

        for (int i = 0; i < normalizedWord.Length; i++)
        {
            if (normalizedWord[i] == inputLetter)
            {
                matchedLetters[i] = true; // Record that the letter at the current position has been matched
                Debug.Log($"Matched letter: {normalizedWord[i]} at position {i}"); // Log the matched letter and position
                matched = true;
            }
        }

        // Build new rich text string with matching letters in green and underscores for unmatched letters
        string richText = "";
        bool allMatched = true;
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (matchedLetters[i])
            {
                richText += $"<color=#90EE90>{currentWord[i]}</color>";
            }
            else
            {
                richText += "_";
                allMatched = false;
            }
        }

        // Log the resulting rich text
        Debug.Log($"Updated rich text: {richText}");

        // Update the WordLabel with the new rich text
        wordLabel.text = richText;

        // Notify the result
        OnWordMatched?.Invoke(allMatched);

        // If no match found, update failed attempts
        if (!matched)
        {
            matchResultManager.UpdateFailedAttempts();
        }
    }

    // Existing Initialize method for backward compatibility
    public void Initialize(string word)
    {
        Initialize(word, WordNormalizer.NormalizeString(word)); // Call the new Initialize method with normalized word
    }
}
