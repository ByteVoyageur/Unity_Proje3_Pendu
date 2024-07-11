using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class NextButtonHandler : MonoBehaviour
{
    private WordGenerator wordGenerator;
    private WordMatcher wordMatcher;
    private MatchResultManager matchResultManager;
    private VisualElement keyboardContainer;

    void Start()
    {
        wordGenerator = GetComponent<WordGenerator>();
        wordMatcher = GetComponent<WordMatcher>();
        matchResultManager = GetComponent<MatchResultManager>();
        keyboardContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("KeyboardButtons");
        
        // Add the click listener for the Next button
        Button nextButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("next-button");
        if (nextButton != null)
        {
            nextButton.clicked += OnNextButtonClick;
        }
    }

    public void OnNextButtonClick()
    {
        Debug.Log("Next button clicked!");

        if (matchResultManager == null)
        {
            Debug.LogError("MatchResultManager is not assigned.");
            return;
        }

        if (matchResultManager.isGameRunning)
        {
            Debug.Log("Game is running. Next button click ignored.");
            return;
        }

        if (keyboardContainer == null)
        {
            Debug.LogError("Keyboard container is not found.");
            return;
        }

        // Request a new word
        StartCoroutine(wordGenerator.GetRandomWordFromAPI());
    }
}