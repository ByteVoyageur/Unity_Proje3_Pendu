using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

        // Display the username in the UserName label
        if (userNameLabel != null)
        {
            string username = PlayerPrefs.GetString("Username", "Guest");
            userNameLabel.text = $"Welcome {username}";
        }
        else
        {
            Debug.LogError("No Label with name 'UserName' found!");
        }

        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'Start' found!");
        }

        logOutButton.clicked += OnLoginOutButtonClicked;

        if (settings != null)
        { 
            settings.clicked += OnSettingsClicked;
        }
        else 
        {
            Debug.LogError ("No Button with name 'Settings' found !");
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

    public void OnLoginOutButtonClicked()
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