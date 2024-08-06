using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using Pendu.LoginPage;
using Pendu.GameStart;

namespace Pendu.MenuPage
{
public class MenuController : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject gameStart;
    public GameObject gameSettings;
    private Button startButton;
    private SoundManager soundManager;
    private Button logOutButton;
    private Button settings;
    private Label userNameLabel;

// Initializes the UI elements, sound manager, and sets up button click event listeners
    void OnEnable()
    {
        // Find the SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        logOutButton = root.Q<Button>("LogOut");
        startButton = root.Q<Button>("Start");
        settings = root.Q<Button>("Settings");
        userNameLabel = root.Q<Label>("UserName");

            if (LoginManager.instance != null)
            {
                if (LoginManager.instance.IsDirectLogin())
                {
                    userNameLabel.text = "Welcome";
                }
                else
                {
                    string username = LoginManager.instance.GetUsername();
                    userNameLabel.text = $"Welcome {username}";
                }
            }
            else
            {
                Debug.LogError("LoginManager instance is null in MenuController OnEnable");
            }

        startButton.clicked += OnStartButtonClicked;
        logOutButton.clicked += OnLogOutButtonClicked;
        settings.clicked += OnSettingsClicked;
    }

// Handles the start button click event, plays the click sound, and switches the UI to the game start screen
    public void OnDisable()
    {
        startButton.clicked -= OnStartButtonClicked;
        logOutButton.clicked -= OnLogOutButtonClicked;
        settings.clicked -= OnSettingsClicked;
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

    public void OnLogOutButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSettingsClicked()
    {
        soundManager.PlayNormalClickSound();
        gameMenu.SetActive(false);
        gameSettings.SetActive(true);
    }
}
}