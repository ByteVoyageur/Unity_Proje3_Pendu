using UnityEngine;
using UnityEngine.UIElements;
using Pendu.MenuPage;

namespace Pendu.GameStart{
public class KeyboardGenerator : MonoBehaviour
{
    private NextButtonHandler nextButtonHandler;
    private MenuController menuController;
    private WordMatcher wordMatcher;
    private SoundManager soundManager;

    void OnEnable()
    {
        // Find the SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager == null)
        {
            Debug.LogError("SoundManager is not found in the scene.");
            return;
        }

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        wordMatcher = GetComponent<WordMatcher>();

        foreach (char letter in alphabet)
        {
            Button button = new Button() { text = letter.ToString() };
            button.name = letter.ToString().ToLower();
            button.AddToClassList("keyboard-button");
            keyboardContainer.Add(button);

            button.clicked += () => HandleButtonClick(button);
        }

        nextButtonHandler = GetComponent<NextButtonHandler>();
        if (nextButtonHandler == null)
        {
            nextButtonHandler = gameObject.AddComponent<NextButtonHandler>();
        }

        // Add a "Next" button
        Button nextButton = new Button() { text = "Next" };
        nextButton.name = "next-button";
        nextButton.AddToClassList("next-button-class");
        keyboardContainer.Add(nextButton);

        // Bind the method from NextButtonHandler and play click sound
        nextButton.clicked += () =>
        {
            soundManager.PlayNormalClickSound(); // Play normal click sound
            nextButtonHandler.OnNextButtonClick();
        };

        // Add a "Return" button
        Button returnButton = new Button() { text = "Return" };
        returnButton.name = "return-button";
        returnButton.AddToClassList("return-button-class");
        keyboardContainer.Add(returnButton);

        // Only play the sound for the Return button click
        returnButton.clicked += () =>
        {
            soundManager.PlayNormalClickSound(); // Play normal click sound
        };
    }

    private void HandleButtonClick(Button button)
    {
        char inputLetter = button.text.ToUpper()[0];
        bool isMatched = wordMatcher.OnButtonClick(inputLetter); // Call WordMatcher to determine if the input letter is a match

        // Play the appropriate sound based on whether the input letter matches
        if (isMatched)
        {
            soundManager.PlaySoundBingo();
        }
        else
        {
            soundManager.PlayErrorClickSound();
        }
    }
}
}