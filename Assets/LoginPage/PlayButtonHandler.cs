using UnityEngine;
using UnityEngine.SceneManagement; // Add this for scene management
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

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
        Debug.Log("Play button clicked! Performing login...");

        // Get the device unique identifier for authentication
        string hardwareId = SystemInfo.deviceUniqueIdentifier;

        // Use the hardware ID to login via PlayFab's LoginWithCustomID method
        LoginManager.instance.LoginWithCustomID(hardwareId, 
        () => {
            Debug.Log("Login with hardwareID successfully.");
            SceneManager.LoadScene(1); // Load the next scene after a successful login
        },
        error => Debug.LogError("Error logging in: " + error.GenerateErrorReport()));
    }
}