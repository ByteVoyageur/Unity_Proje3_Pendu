using UnityEngine;
using UnityEngine.UIElements;
using Pendu.GameStart;

public class ReturnToMenuHandler : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject gameSettings;
    private UnityEngine.UIElements.Button returnToMenu;
    private SoundManager soundManager;

    void OnEnable()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found!");
            return;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;

        returnToMenu = root.Q<UnityEngine.UIElements.Button>("ReturnToMenu");
        if (returnToMenu != null)
        {
            returnToMenu.clicked += OnReturnToMenuClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'ReturnToMenu' found!");
        }
    }

    void OnReturnToMenuClicked()
    {
        if (gameMenu != null && gameSettings != null)
        {
            soundManager.PlayNormalClickSound();
            gameMenu.SetActive(true);   // Activate the menu
            gameSettings.SetActive(false); // Deactivate the settings
        }
        else
        {
            Debug.LogError("GameMenu or GameSettings GameObject not assigned.");
        }
    }

    void OnDisable()
    {
        if (returnToMenu != null)
        {
            returnToMenu.clicked -= OnReturnToMenuClicked;
        }
    }
}
