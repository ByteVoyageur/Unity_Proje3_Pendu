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

        // Update the labels with the initial scores
        UpdateScoreLabels();
    }

    public void IncrementWinCount()
    {
        winCount++;
        UpdateScoreLabels();
    }

    public void IncrementLoseCount()
    {
        loseCount++;
        UpdateScoreLabels();
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
}