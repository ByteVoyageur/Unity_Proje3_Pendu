using UnityEngine;
using UnityEngine.UIElements;

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
        LoginManager.instance.LoadUserStats(OnStatsLoaded);
    }

    public void IncrementWinCount()
    {
        winCount++;
        UpdateScoreLabels();
        SaveStats();
    }

    public void IncrementLoseCount()
    {
        loseCount++;
        UpdateScoreLabels();
        SaveStats();
    }

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

    private void OnStatsLoaded(int loadedWinCount, int loadedLoseCount)
    {
        winCount = loadedWinCount;
        loseCount = loadedLoseCount;
        UpdateScoreLabels();
    }

    private void SaveStats()
    {
        LoginManager.instance.SaveUserStats(winCount, loseCount);
    }
}