using UnityEngine;
using UnityEngine.UI;
using YG;

public class ScoreSaver : MonoBehaviour
{
    public static ScoreSaver Instance { get; private set; }

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
        int audioHighScore = YG2.saves.AudioHighScore;
        int imageHighScore = YG2.saves.ImageHighScore;
        int textHighScore = YG2.saves.TextHighScore;

        int totalScore = audioHighScore + imageHighScore + textHighScore;

        int savedTotalHighScore = YG2.saves.TotalScore;

        if (totalScore > savedTotalHighScore)
        {
            YG2.saves.TotalScore = totalScore;
            YG2.SetLeaderboard(LeaderboardName, totalScore);

            YG2.SaveProgress();
        }
    }
}