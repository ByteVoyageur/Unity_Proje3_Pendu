using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject gameStart;
    private Button startButton;
    private SoundManager soundManager;
    private Button logOutButton;

    void OnEnable()
    {
        // Find the SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        logOutButton = root.Q<Button>("LogOut");
        startButton = root.Q<Button>("Start");

        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'Start' found!");
        }

        logOutButton.clicked += OnLoginOutButtonClicked;
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

}