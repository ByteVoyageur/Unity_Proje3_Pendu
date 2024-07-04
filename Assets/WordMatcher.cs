using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WordMatcher : MonoBehaviour
{
    private string currentWord;
    private Label wordLabel;
    private VisualElement keyboardContainer;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        wordLabel = root.Q<Label>("WordLabel");
        keyboardContainer = root.Q<VisualElement>("InputLettre");

        // Get the current word from the WordLabel
        currentWord = wordLabel.text;
        Debug.Log($"Current Word: {currentWord}"); // Log the current word

        // Add button click listeners for all keyboard buttons
        foreach (VisualElement element in keyboardContainer.Children())
        {
            if (element is Button button)
            {
                button.clicked += () =>
                {
                    char inputLetter = button.text.ToUpper()[0];
                    Debug.Log($"Button Clicked: {inputLetter}"); // Log the clicked letter
                    OnButtonClick(inputLetter);
                };
            }
        }
    }

    private void OnButtonClick(char inputLetter)
    {
        Debug.Log($"Checking input letter: {inputLetter} against word: {currentWord}"); // Log the input letter and current word
        char[] displayedWordArray = new char[currentWord.Length];

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord[i] == inputLetter)
            {
                displayedWordArray[i] = currentWord[i];
            }
            else
            {
                displayedWordArray[i] = ' '; // Use space to preserve original positions
            }
        }

        // Build new rich text string with matching letters in green
        string richText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (displayedWordArray[i] == currentWord[i])
            {
                richText += $"<color=green>{currentWord[i]}</color>";
                Debug.Log($"Matched letter: {currentWord[i]} at position {i}"); // Log the matched letter and position
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