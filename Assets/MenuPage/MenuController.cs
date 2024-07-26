using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;

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

    void OnEnable()
    {
        // Find the SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        logOutButton = root.Q<Button>("LogOut");
        startButton = root.Q<Button>("Start");
        settings = root.Q<Button>("Settings");
        userNameLabel = root.Q<Label>("UserName");

        if (LoginManager.instance.IsDirectLogin())
        {
            userNameLabel.text = "Welcome";
        }
        else
        {
            string username = LoginManager.instance.GetUsername();
            userNameLabel.text = $"Welcome {username}";
        }

        startButton.clicked += OnStartButtonClicked;
        logOutButton.clicked += OnLogOutButtonClicked;
        settings.clicked += OnSettingsClicked;
    }

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