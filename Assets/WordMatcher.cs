using UnityEngine;
using UnityEngine.UIElements;

public class WordMatcher : MonoBehaviour
{
    private string currentWord;
    private Label wordLabel;
    private VisualElement keyboardContainer;
    private bool[] matchedLetters; // Array to keep track of matched letters

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
                    char inputLetter = button.text.ToUpper()[0]; // Convert to uppercase
                    Debug.Log($"Button Clicked: {inputLetter}"); 
                    OnButtonClick(inputLetter);
                };
            }
        }
    }

    public void Initialize(string word)
    {
        currentWord = word.ToUpper(); // Convert to uppercase
        matchedLetters = new bool[currentWord.Length]; // Initialize matchedLetters array
        Debug.Log($"Initialized with word: {currentWord}");
    }

    private void OnButtonClick(char inputLetter)
    {
        Debug.Log($"Checking input letter: {inputLetter} against word: {currentWord}"); // Log the input letter and current word

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord[i] == inputLetter)
            {
                matchedLetters[i] = true; // Record that the letter at the current position has been matched
                Debug.Log($"Matched letter: {currentWord[i]} at position {i}"); // Log the matched letter and position
            }
        }

        // Build new rich text string with matching letters in green
        string richText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (matchedLetters[i])
            {
                richText += $"<color=#90EE90>{currentWord[i]}</color>";
            }
            else
            {
                richText += currentWord[i];
            }
        }

        // Log the resulting rich text
        Debug.Log($"Updated rich text: {richText}");

        // Update the WordLabel with the new rich text
        wordLabel.text = richText;
    }
}
