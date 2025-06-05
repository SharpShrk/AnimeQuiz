using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;

public class Leaderboard : MonoBehaviour
{
    private const string LeaderboardName = "AnimeQuiz";

    [Header("Settings")]
    [SerializeField] private LeaderboardYG _leaderboardYGComponent;
    [SerializeField] private Button _openLeaderboardButton;
    [SerializeField] private Button _closeLeaderboardButton;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private GameObject _gameUI;

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

    private void Start()
    {
        YG2.GetLeaderboard(LeaderboardName);
    }

    private void OnEnable()
    {
        _openLeaderboardButton.onClick.AddListener(OpenLeaderboardPanel);
        _closeLeaderboardButton.onClick.AddListener(CloseLeaderboardPanel);
        YG2.onGetLeaderboard += OnUpdateLB;
    }

    private void OnDisable()
    {
        _openLeaderboardButton.onClick.RemoveAllListeners();
        _closeLeaderboardButton.onClick.RemoveAllListeners();
        YG2.onGetLeaderboard -= OnUpdateLB;
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
        _playerDataObject.SetActive(false);
        YG2.GetLeaderboard(LeaderboardName);
        _leaderboardYGComponent.UpdateLB();

        _gameUI.SetActive(false);
        _leaderboardPanel.SetActive(true);
        _playerDataObject.SetActive(_currentPlayerData.rank >= 9);

        _rank.text = _playerRank;
        _score.text = _playerScore;

        if (YG2.player.auth)
        {
            _name.text = _playerName;
        }
        else
        {
            if (YG2.envir.language == "ru")
                _name.text = "Аноним";
            else
                _name.text = "Anonymous";
        }
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