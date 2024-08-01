using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

namespace Pendu.GameStart{
public class NextButtonHandler : MonoBehaviour
{
    private WordGenerator wordGenerator;
    private WordMatcher wordMatcher;
    private MatchResultManager matchResultManager;
    private KeyboardStatusManager keyboardStatusManager;

    void Start()
    {
        wordGenerator = GetComponent<WordGenerator>();
        wordMatcher = GetComponent<WordMatcher>();
        matchResultManager = GetComponent<MatchResultManager>();
        keyboardStatusManager = GetComponent<KeyboardStatusManager>(); // Initialize KeyboardStatusManager
        
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No UIDocument component found!");
            return;
        }
        
        var root = uiDocument.rootVisualElement;
        var keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        // Add the click listener for the Next button
        var nextButton = root.Q<Button>("next-button");
        if (nextButton != null)
        {
            nextButton.clicked += OnNextButtonClick;
        }
    }

    public void OnNextButtonClick()
    {

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

        // Reset keyboard buttons state
        if (keyboardStatusManager != null)
        {
            keyboardStatusManager.ResetKeyboardStatus();
        }
        else
        {
            Debug.LogError("KeyboardStatusManager is not assigned.");
        }

        // Request a new word
        StartCoroutine(wordGenerator.GetRandomWordFromAPI());
    }
}
}
