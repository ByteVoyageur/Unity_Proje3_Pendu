using UnityEngine;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    private Button signInWithButton;
    private VisualElement loginDialog;
    private TextField usernameField;
    private Button submitUsernameButton;
    private Button closeDialogButton; 

    void OnEnable()
    {
        // Get the root visual element from the UI Document
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the Sign In With button and login dialog elements
        signInWithButton = root.Q<Button>("SignInWith");
        loginDialog = root.Q<VisualElement>("LoginDialog");
        usernameField = loginDialog.Q<TextField>("UsernameField");
        submitUsernameButton = loginDialog.Q<Button>("SubmitUsername");
        closeDialogButton = loginDialog.Q<Button>("CloseDialog"); 

        if (signInWithButton != null)
        {
            // Add click event handler to the Sign In With button
            signInWithButton.clicked += OnSignInWithButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'SignInWith' found!");
        }

        if (submitUsernameButton != null)
        {
            // Add click event handler to the Submit Username button
            submitUsernameButton.clicked += OnSubmitUsernameClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'SubmitUsername' found!");
        }

        if (closeDialogButton != null)
        {
            // Add click event handler to the Close button
            closeDialogButton.clicked += OnCloseDialogButtonClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'CloseDialog' found!");
        }

        // Hide the login dialog initially
        loginDialog.style.display = DisplayStyle.None;
    }

    private void OnSignInWithButtonClicked()
    {
        Debug.Log("Sign In With button clicked! Showing login dialog...");
        // Show the login dialog
        loginDialog.style.display = DisplayStyle.Flex;
    }

    private void OnSubmitUsernameClicked()
    {
        string username = usernameField.text;
        if (!string.IsNullOrEmpty(username))
        {
            Debug.Log("Username submitted: " + username);

            // Hide the login dialog
            loginDialog.style.display = DisplayStyle.None;

            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
            {
                CustomId = username,
                CreateAccount = true // Create an account if it does not exist
            };

            PlayFabClientAPI.LoginWithCustomID(request, result =>
            {
                Debug.Log("Login PlayFab with username success!");
                PlayerPrefs.SetString("Username", username);
                PlayerPrefs.SetInt("IsLoggedIn", 1);

                // Update the display name after successful login
                UpdateUserDisplayName(username);

            }, OnLoginFailure);
        }
        else
        {
            Debug.LogError("Username is empty. Please enter a valid username.");
        }
    }

    private void UpdateUserDisplayName(string username)
    {
        var updateRequest = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(updateRequest, 
            result => 
            { 
                Debug.Log("User display name updated successfully.");
                // Perform actions after a successful update
                OnLoginSuccess();
            }, 
            error => 
            { 
                Debug.LogError("Error updating user display name: " + error.GenerateErrorReport());
                // Handle the error, for example showing a message to the user
            });
    }

    private void OnCloseDialogButtonClicked()
    {
        Debug.Log("Close button clicked! Hiding login dialog...");
        // Hide the login dialog
        loginDialog.style.display = DisplayStyle.None;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.GenerateErrorReport());
        // Handle login failure, e.g., notify the user
    }

    private void OnLoginSuccess()
    {
        // Load the main game scene
        SceneManager.LoadScene(1); 
    }
}