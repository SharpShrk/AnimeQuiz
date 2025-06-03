using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    private const string AudioQuizHighScoreKey = "AudioQuizHighScore";
    private const string ImageQuizHighScoreKey = "ImageQuizHighScore";
    private const string TextQuizHighScoreKey = "TextQuizHighScore";
    private const string LeaderboardScoreKey = "TotalHighScore";
    private const string LeaderboardName = "AnimeQuiz";

    [Header("Settings")]
    [SerializeField] private LeaderboardYG _leaderboardYGComponent;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private Button _openLeaderboardButton;
    [SerializeField] private Button _closeLeaderboardButton;

    [Header("PlayerData")]
    [SerializeField] private GameObject _playerDataObject;
    [SerializeField] private Text _rank;
    [SerializeField] private Text _name;
    [SerializeField] private Text _score;

    private LBCurrentPlayerData _currentPlayerData;
    private Sprite _playerPhotoSprite;
    private string _playerRank;
    private string _playerScore;
    private string _playerName;

    public Sprite PlayerPhotoSprite => _playerPhotoSprite;

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

    private void OnEnable()
    {
        _openLeaderboardButton.onClick.AddListener(OpenLeaderboardPanel);
        _closeLeaderboardButton.onClick.AddListener(CloseLeaderboardPanel);

        YG2.onGetLeaderboard += OnUpdateLB;
        YG2.GetLeaderboard(LeaderboardName);
    }

    private void OnDisable()
    {
        _openLeaderboardButton.onClick.RemoveListener(OpenLeaderboardPanel);
        _closeLeaderboardButton.onClick.RemoveListener(CloseLeaderboardPanel);

        YG2.onGetLeaderboard -= OnUpdateLB;
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
            SaveToLeaderboard(totalScore);
        }
    }

    private void SaveToLeaderboard(int score)
    {
        YG2.SetLeaderboard(LeaderboardName, score);
    }

    private void OnUpdateLB(LBData lbData)
    {
        if (lbData.technoName == LeaderboardName)
        {
            _currentPlayerData = lbData.currentPlayer;

            _playerRank = _currentPlayerData.rank.ToString();
            _playerScore = _currentPlayerData.score.ToString();
            _playerName = YG2.player.name;

            GetPlayerPhoto();
        }
    }

    private void GetPlayerPhoto()
    {
        string photoUrl = YG2.player.photo;

        if (!string.IsNullOrEmpty(photoUrl))
        {
            StartCoroutine(LoadPlayerPhoto(photoUrl));
        }
    }

    private void OpenLeaderboardPanel()
    {
        _leaderboardYGComponent.UpdateLB();

        _gameUI.SetActive(false);
        _leaderboardPanel.SetActive(true);
        _playerDataObject.SetActive(_currentPlayerData.rank >= 9);

        _rank.text = _playerRank;
        _score.text = _playerScore;
        _name.text = _playerName;
    }

    private void CloseLeaderboardPanel()
    {
        _gameUI.SetActive(true);
        _leaderboardPanel.SetActive(false);
    }

    private IEnumerator LoadPlayerPhoto(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка загрузки аватара: {uwr.error}");
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                if (texture != null)
                {
                    _playerPhotoSprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                }
            }
        }
    }
}
