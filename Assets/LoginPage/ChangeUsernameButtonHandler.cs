using UnityEngine;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class ChangeUsernameButtonHandler : MonoBehaviour
{
    private Button signInWithButton;
    private VisualElement loginDialog;
    private TextField usernameField;
    private Button submitUsernameButton;
    private Button closeDialogButton;
    private string username;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        signInWithButton = root.Q<Button>("SignInWith");
        loginDialog = root.Q<VisualElement>("LoginDialog");
        usernameField = loginDialog.Q<TextField>("UsernameField");
        submitUsernameButton = loginDialog.Q<Button>("SubmitUsername");
        closeDialogButton = loginDialog.Q<Button>("CloseDialog");

        signInWithButton.clicked += OnSignInWithButtonClicked;
        submitUsernameButton.clicked += OnSubmitUsernameClicked;
        closeDialogButton.clicked += OnCloseDialogButtonClicked;
        loginDialog.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        signInWithButton.clicked -= OnSignInWithButtonClicked;
        submitUsernameButton.clicked -= OnSubmitUsernameClicked;
        closeDialogButton.clicked -= OnCloseDialogButtonClicked;
    }

    private void OnSignInWithButtonClicked()
    {
        loginDialog.style.display = DisplayStyle.Flex;
    }

    private void OnSubmitUsernameClicked()
    {
        username = usernameField.text;
        if (!string.IsNullOrEmpty(username))
        {
            loginDialog.style.display = DisplayStyle.None;
            LoginWithCustomID(username);
        }
        else
        {
            Debug.LogError("Username is empty. Please enter a valid username.");
        }
    }

    private void OnCloseDialogButtonClicked()
    {
        loginDialog.style.display = DisplayStyle.None;
    }

    private void LoginWithCustomID(string customID)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customID,
            CreateAccount = true // Create account if it doesn't exist
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        LoginManager.instance.SetUsername(username);  
        SceneManager.LoadScene(1);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Error logging in: " + error.GenerateErrorReport());
    }
}