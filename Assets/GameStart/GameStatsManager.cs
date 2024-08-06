using UnityEngine;
using UnityEngine.UIElements;
using Pendu.LoginPage;

namespace Pendu.GameStart{
    /// <summary>
    /// Manages the game statistics, including win and loss counts, Updates the UI to reflect these stats
    /// and handles loading and saving stats to PlayFab.
    /// </summary>
public class GameStatsManager : MonoBehaviour
{
    private int winCount = 0;
    private int loseCount = 0;

    private Label scoreWinLabel;
    private Label scoreFailedLabel;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Fetch the labels
        scoreWinLabel = root.Q<Label>("ScoreWin");
        scoreFailedLabel = root.Q<Label>("ScoreFailed");

        // Load the scores from PlayFab
        LoadStats();
    }

//Updates the score win.
    public void IncrementWinCount()
    {
        winCount++;
        UpdateScoreLabels();
        SaveStats();
    }

// Updates the score loss.
    public void IncrementLoseCount()
    {
        loseCount++;
        UpdateScoreLabels();
        SaveStats();
    }

// when called stats are successfully loaded from PlayFab, updates local counts and UI labels
    private void UpdateScoreLabels()
    {
        if (scoreWinLabel != null)
        {
            scoreWinLabel.text = $"Wins: {winCount}";
        }

        if (scoreFailedLabel != null)
        {
            scoreFailedLabel.text = $"Losses: {loseCount}";
        }
    }

    private void OnStatsLoaded()
    {
        winCount = LoginManager.instance.winCount;
        loseCount = LoginManager.instance.loseCount;
        UpdateScoreLabels();
    }

// Initiates the loading of the user stats from PlayFab throuth LoginManager.
    private void LoadStats()
    {
        LoginManager.instance.LoadUserStats(OnStatsLoaded);
    }

// Initiates the saving of the current user stats to PlayFab through LoginManager.
    private void SaveStats()
    {
        LoginManager.instance.SaveUserStats(winCount, loseCount);
    }
}
}