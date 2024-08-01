
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

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

    private void OnDisable()
    {
        signInWithButton.clicked -= OnSignInWithButtonClicked;
        submitUsernameButton.clicked -= OnSubmitUsernameClicked;
        closeDialogButton.clicked -= OnCloseDialogButtonClicked;
    }

    private void OnSignInWithButtonClicked()
    {
        loginDialog.style.display = DisplayStyle.Flex;
        loginDialog.AddToClassList("login-dialog--down");
    }

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

    private void OnCloseDialogButtonClicked()
    {
        loginDialog.RemoveFromClassList("login-dialog--down");
        loginDialog.style.display = DisplayStyle.None;
    }
}
}