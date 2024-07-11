using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Globalization;
using System.Text;

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

        // Add button click listeners for all keyboard buttons
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button)
            {
                button.clicked += () =>
                {
                    if (!inputDisabled)
                    {
                        char inputLetter = button.text.ToUpper()[0]; // Convert to uppercase
                        Debug.Log($"Button Clicked: {inputLetter}"); 
                        OnButtonClick(inputLetter);
                    }
                };
            }
        }

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
    private void OnButtonClick(char inputLetter)
    {
        Debug.Log($"Checking input letter: {inputLetter} against word: {normalizedWord}"); // Log the input letter and normalized word

        bool isMatched = false;
        for (int i = 0; i < normalizedWord.Length; i++)
        {
            if (normalizedWord[i] == inputLetter)
            {
                matchedLetters[i] = true; // Record that the letter at the current position has been matched
                Debug.Log($"Matched letter: {normalizedWord[i]} at position {i}"); // Log the matched letter and position
                isMatched = true;
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
    }

    // Existing Initialize method for backward compatibility
    public void Initialize(string word)
    {
        Initialize(word, NormalizeString(word)); // Call the new Initialize method with normalized word
    }

    // Normalize the string to remove diacritics and convert to uppercase
    private string NormalizeString(string input)
    {
        string normalizedString = input.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper(); // Ensure the string is uppercase
    }
}

