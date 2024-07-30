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

    public bool IsDirectLogin()
    {
        return isDirectLogin;
    }

    public void SetUsername(string username)
    {
        this.username = username;
    }

    public string GetUsername()
    {
        return username;
    }

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