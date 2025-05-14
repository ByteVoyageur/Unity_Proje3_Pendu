
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Pendu.LoginPage
{
public class ChangeUsernameButtonHandler : MonoBehaviour
{
    private Button signInWithButton;
    private VisualElement loginDialog;
    private TextField usernameField;
    private Button submitUsernameButton;
    private Button closeDialogButton;
    private string username;

// Initializes the UI elements and sets up button click event listeners
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
    }

  // Cleans up event listeners when the object is disabled
    private void OnDisable()
    {
        signInWithButton.clicked -= OnSignInWithButtonClicked;
        submitUsernameButton.clicked -= OnSubmitUsernameClicked;
        closeDialogButton.clicked -= OnCloseDialogButtonClicked;
    }

// Displays the login dialog and adds animation class to it
    private void OnSignInWithButtonClicked()
    {
        loginDialog.style.display = DisplayStyle.Flex;
        loginDialog.AddToClassList("login-dialog--down");
    }

// Handles the submit username button click event, logs in the user and loads a new scene upon success
    private void OnSubmitUsernameClicked()
    {
        username = usernameField.text;
        if (!string.IsNullOrEmpty(username))
        {
            loginDialog.style.display = DisplayStyle.None;
            LoginManager.instance.LoginWithCustomID(username);
            LoginManager.instance.SetUsername(username);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Username is empty. Please enter a valid username.");
        }
    }

// Closes the login dialog and removes the animation class
    private void OnCloseDialogButtonClicked()
    {
        loginDialog.RemoveFromClassList("login-dialog--down");
        loginDialog.style.display = DisplayStyle.None;
    }
}
}