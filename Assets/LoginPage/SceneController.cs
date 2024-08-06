using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Pendu.LoginPage
{
public class SceneController : MonoBehaviour
{
    private VisualElement root;

    // Initializes the UI and sets up button click event listeners
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        var playButton = root.Q<Button>("PLAY");
        if (playButton == null)
        {
            Debug.LogError("Button with name 'PLAY' not found.");
            return;
        }

        playButton.clicked += OnPlayButtonClicked;
    }

    // Handles the play button click event and loads a new scene if the user is logged in
    public void OnPlayButtonClicked()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("User is not logged in!");
        }
    }
}
}