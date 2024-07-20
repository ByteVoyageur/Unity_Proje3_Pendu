using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject gameStart;
    private Button startButton;
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

        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("Start");

        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'Start' found!");
        }
    }

    public void OnStartButtonClicked()
    {
        soundManager.PlayNormalClickSound(); 
        gameMenu.SetActive(false);
        gameStart.SetActive(true);
    }

    public void OnJeuClicked()
    {
        gameMenu.SetActive(true);
        gameStart.SetActive(false);
    }
}