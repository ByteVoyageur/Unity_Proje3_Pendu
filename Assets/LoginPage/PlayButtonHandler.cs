using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Pendu.LoginPage
{
public class PlayButtonHandler : MonoBehaviour
{
    private Button playButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("PLAY");

        playButton.clicked += OnPlayButtonClicked;
    }

    // Play button click event, logs in the user with hardware ID, and loads a new scene upon success

    private void OnPlayButtonClicked()
    {
        string hardwareId = SystemInfo.deviceUniqueIdentifier;

        LoginManager.instance.LoginWithHardwareID(hardwareId, 
        () => {
            SceneManager.LoadScene(1); 
        },
        error => Debug.LogError("Error logging in: " + error.GenerateErrorReport()));
    }
}
}