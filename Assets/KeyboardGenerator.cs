using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardGenerator : MonoBehaviour
{
    private NextButtonHandler nextButtonHandler;
    private MenuController menuController;
    private WordMatcher wordMatcher;

    void OnEnable()
    {
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

        // Find the NextButtonHandler component
        nextButtonHandler = GetComponent<NextButtonHandler>();
        if (nextButtonHandler == null)
        {
            nextButtonHandler = gameObject.AddComponent<NextButtonHandler>(); // ensure NextButtonHandler exists
        }

        // Add a "Next" button
        Button nextButton = new Button() { text = "Next" };
        nextButton.name = "next-button";
        nextButton.AddToClassList("next-button-class");
        keyboardContainer.Add(nextButton);

        // Bind the method from NextButtonHandler
        nextButton.clicked += nextButtonHandler.OnNextButtonClick;

        // Add a "Return" button
        Button returnButton = new Button() { text = "Return" };
        returnButton.name = "return-button";
        returnButton.AddToClassList("return-button-class");
        keyboardContainer.Add(returnButton);

        // Find the MenuController component in the parent or root GameObject
        
    }

    private void HandleButtonClick(Button button)
    {
        char inputLetter = button.text.ToUpper()[0];
        wordMatcher.OnButtonClick(inputLetter);
    }
}
