using UnityEngine;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class LoginPlayDirect : MonoBehaviour
{
    private Button playButton;

    void OnEnable()
    {
        // Get the root visual element from the UI Document
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the Play button
        playButton = root.Q<Button>("PLAY");

        if (playButton != null)
        {
            // Add click event handler to the Play button
            playButton.clicked += OnPlayButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'PLAY' found!");
        }
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked! Performing login...");

        // Get the device unique identifier for authentication
        string hardwareId = SystemInfo.deviceUniqueIdentifier;

        // Use the hardware ID to login via PlayFab's LoginWithCustomID method
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = hardwareId,
            CreateAccount = true // Create an account if it does not exist
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login PlayFab with hardwareID success!");
        // Mark the user as logged in
        PlayerPrefs.SetInt("IsLoggedIn", 1);
        
        // Perform actions after a successful login, e.g., load the main game scene
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
        // Handle login failure, e.g., notify the user
    }
}