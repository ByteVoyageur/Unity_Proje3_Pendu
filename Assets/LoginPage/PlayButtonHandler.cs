using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayButtonHandler : MonoBehaviour
{
    private Button playButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("PLAY");

        playButton.clicked += OnPlayButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        string hardwareId = SystemInfo.deviceUniqueIdentifier;

        LoginManager.instance.LoginWithHardwareID(hardwareId, 
        () => {
            Debug.Log("Login with hardwareID successfully.");
            SceneManager.LoadScene(1); 
        },
        error => Debug.LogError("Error logging in: " + error.GenerateErrorReport()));
    }
}