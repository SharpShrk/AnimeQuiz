using UnityEngine;
using UnityEngine.UI;
using YG;

public class ScoreSaver : MonoBehaviour
{
    public static ScoreSaver Instance { get; private set; }

    private const string AudioQuizHighScoreKey = "AudioQuizHighScore";
    private const string ImageQuizHighScoreKey = "ImageQuizHighScore";
    private const string TextQuizHighScoreKey = "TextQuizHighScore";
    private const string LeaderboardScoreKey = "TotalHighScore";
    private const string LeaderboardName = "AnimeQuiz";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TrySaveHighScore()
    {
        int audioHighScore = YG2.GetState(AudioQuizHighScoreKey);
        int imageHighScore = YG2.GetState(ImageQuizHighScoreKey);
        int textHighScore = YG2.GetState(TextQuizHighScoreKey);

        int totalScore = audioHighScore + imageHighScore + textHighScore;

        int savedTotalHighScore = YG2.GetState(LeaderboardScoreKey);

        if (totalScore > savedTotalHighScore)
        {
            YG2.SetState(LeaderboardScoreKey, totalScore);
            YG2.SetLeaderboard(LeaderboardName, totalScore);
        }
    }
}