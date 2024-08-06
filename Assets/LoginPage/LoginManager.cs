using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Pendu.LoginPage
{
public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;
    private bool isDirectLogin = false; 
    private string username;
    public int winCount = 0;
    public int loseCount = 0;

// Initializes the instance of the LoginManager and ensures it's not destroyed on load
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("LoginManager instance set and DontDestroyOnLoad called.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate LoginManager instance destroyed.");
        }
    }

// Logs in using hardware ID and handles the success or error through callbacks
    public void LoginWithHardwareID(string hardwareID, System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        isDirectLogin = true; 
        var request = new LoginWithCustomIDRequest
        {
            CustomId = hardwareID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,
            result => {
                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error logging in: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }

// Logs in using custom ID and handles the success or error through callbacks
        public void LoginWithCustomID(string customID, System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        isDirectLogin = false; 
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,
            result => {
                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error logging in: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }

 // Updates the user's display name and handles the success or error through callbacks
    public void UpdateDisplayName(string newDisplayName, System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => {
                username = newDisplayName;
                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error updating display name: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }

// Returns whether the login is direct
    public bool IsDirectLogin()
    {
        return isDirectLogin;
    }

// Sets the username for the user
    public void SetUsername(string username)
    {
        this.username = username;
    }

// Gets the username of the user
    public string GetUsername()
    {
        return username;
    }

// Saves user statistics (win and lose counts) and handles the success or error through callbacks
        public void SaveUserStats(int winCount, int loseCount, System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "WinCount", winCount.ToString() },
                { "LoseCount", loseCount.ToString() }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => {
                Debug.Log("User stats updated successfully.");
                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error updating user stats: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }

 // Loads user statistics (win and lose counts) and handles the success or error through callbacks
    public void LoadUserStats(System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result => {

                if (result.Data != null && result.Data.ContainsKey("WinCount"))
                {
                    winCount = int.Parse(result.Data["WinCount"].Value);
                }

                if (result.Data != null && result.Data.ContainsKey("LoseCount"))
                {
                    loseCount = int.Parse(result.Data["LoseCount"].Value);
                }

                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }
}
}