using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;
    private string username;
    private bool loggedIn = false; // Add this flag to indicate the login status

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoginWithCustomID(string customID, System.Action onSuccess = null, System.Action<PlayFabError> onError = null)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,
            result => {
                Debug.Log("Login successful!");
                loggedIn = true; // Update the flag on successful login
                onSuccess?.Invoke();
            },
            error => {
                Debug.LogError("Error logging in: " + error.GenerateErrorReport());
                onError?.Invoke(error);
            }
        );
    }

    public bool IsLoggedIn()
    {
        return loggedIn;
    }

    public void UpdateUsername(string newUsername)
    {
        username = newUsername;
        var updateRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = newUsername };

        PlayFabClientAPI.UpdateUserTitleDisplayName(updateRequest,
            result => Debug.Log("User display name updated successfully."),
            error => Debug.LogError("Error updating user display name: " + error.GenerateErrorReport())
        );
    }

    public string GetUsername()
    {
        return username;
    }

    public void SaveUserStats(int winCount, int loseCount)
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
            result => Debug.Log("User stats updated successfully."),
            error => Debug.LogError("Error updating user stats: " + error.GenerateErrorReport())
        );
    }

    public void LoadUserStats(System.Action<int, int> onStatsLoaded = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), 
            result => {
                Debug.Log("Received user data!");
                int winCount = result.Data.ContainsKey("WinCount") ? int.Parse(result.Data["WinCount"].Value) : 0;
                int loseCount = result.Data.ContainsKey("LoseCount") ? int.Parse(result.Data["LoseCount"].Value) : 0;

                onStatsLoaded?.Invoke(winCount, loseCount);
            },
            error => Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport())
        );
    }

    public void Logout()
    {
        loggedIn = false;
        username = null;
        // Perform additional logout actions if necessary
        Debug.Log("Logged out successfully.");
    }
}