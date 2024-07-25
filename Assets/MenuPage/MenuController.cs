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

        // Ensure successful login before retrieving the username
        if (LoginManager.instance.IsLoggedIn())
        {
            GetAndDisplayUsername();
        }
        else
        {
            Debug.LogError("User not logged in. Cannot retrieve account info.");
            // Optionally, redirect to login scene
            SceneManager.LoadScene(0);
        }

        startButton.clicked += OnStartButtonClicked;
        logOutButton.clicked += OnLogOutButtonClicked;
        settings.clicked += OnSettingsClicked;
    }

    private void GetAndDisplayUsername()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
            result => {
                string displayName = result.AccountInfo.TitleInfo.DisplayName;
                userNameLabel.text = $"Welcome {displayName}";
            },
            error => {
                Debug.LogError("Error retrieving account info: " + error.GenerateErrorReport());
                userNameLabel.text = "Welcome"; // Display a default message if unable to get the username
            });
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
        LoginManager.instance.Logout();
        SceneManager.LoadScene(0);
    }

    public void OnSettingsClicked()
    {
        soundManager.PlayNormalClickSound();
        gameMenu.SetActive(false);
        gameSettings.SetActive(true);
    }
}