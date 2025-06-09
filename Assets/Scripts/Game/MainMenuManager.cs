using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using YG;
using YG.Insides;

public class MainMenuManager : MonoBehaviour
{
    private static readonly string[] _ruLocale = { "ru", "be", "kk", "uk", "uz" };

    private const string TextQuizSceneName = "TextQuizScene";
    private const string ImageQuizSceneName = "ImageQuizScene";
    private const string AudioQuizSceneName = "AudioQuizScene";

    [SerializeField] private Autorization _autorization;
    [SerializeField] private TMP_Text _imageQuizScoreText;
    [SerializeField] private TMP_Text _audioQuizScoreText;
    [SerializeField] private TMP_Text _textQuizScoreText;
    [SerializeField] private Button _startTextQuizButton;
    [SerializeField] private Button _startImageQuizButton;
    [SerializeField] private Button _startAudioQuizButton;

    private bool _isRu;

    private void OnEnable()
    {
        _startTextQuizButton.onClick.AddListener(StartTextQuiz);
        _startImageQuizButton.onClick.AddListener(StartImageQuiz);
        _startAudioQuizButton.onClick.AddListener(StartAudioQuiz);
    }

    private void OnDisable()
    {
        _startImageQuizButton.onClick.RemoveListener(StartImageQuiz);
        _startTextQuizButton.onClick.RemoveListener(StartTextQuiz);
        _startAudioQuizButton.onClick.RemoveListener(StartAudioQuiz);
    }

    private void Start()
    {
        YG2.onGetSDKData += OnDataLoaded;
        CheckLanguage();
        YGInsides.LoadProgress();

        if (!YG2.player.auth)
            _autorization.OpenAutorizationPanel(); 
    }

    private void OnDataLoaded()
    {
        YG2.onGetSDKData -= OnDataLoaded;
        LoadHighScores();
    }

    private void CheckLanguage()
    {
        _isRu = false;

        foreach (var locale in _ruLocale)
        {
            if (locale == YG2.lang)
            {
                _isRu = true;
                break;
            }
        }
    }

    private void StartImageQuiz()
    {
        SceneManager.LoadScene(ImageQuizSceneName);
    }

    private void StartTextQuiz()
    {
        SceneManager.LoadScene(TextQuizSceneName);
    }

    private void StartAudioQuiz()
    {
        SceneManager.LoadScene(AudioQuizSceneName);
    }

    private void LoadHighScores()
    {
        _imageQuizScoreText.text = SetColorScore(YG2.saves.ImageHighScore);
        _audioQuizScoreText.text = SetColorScore(YG2.saves.AudioHighScore);
        _textQuizScoreText.text = SetColorScore(YG2.saves.TextHighScore);
    }

    private string SetColorScore(int score)
    {
        string colorScore = "";

        if (_isRu)
            colorScore = $"Рекорд: <color=#E9BA00><b>{score}</b></color>";
        else
            colorScore = $"Score: <color=#E9BA00><b>{score}</b></color>";

        return colorScore;
    }
}