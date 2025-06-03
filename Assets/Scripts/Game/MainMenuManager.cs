using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using YG;

public class MainMenuManager : MonoBehaviour
{
    private static readonly string[] _ruLocale = { "ru", "be", "kk", "uk", "uz" };

    private const string TextQuizScoreKey = "TextQuizHighScore";
    private const string ImageQuizScoreKey = "ImageQuizHighScore";
    private const string AudioQuizScoreKey = "AudioQuizHighScore";
    private const string TextQuizSceneName = "TextQuizScene";
    private const string ImageQuizSceneName = "ImageQuizScene";
    private const string AudioQuizSceneName = "AudioQuizScene";
    private const string IsFirstStartName = "IsFirstStart";

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
    }

    private void Start()
    {
        if (!YG2.player.auth)
            _autorization.OpenAutorizationPanel();

        CheckLanguage();
        FirstStartCheck();
        LoadHighScores();
    }

    private void FirstStartCheck()
    {
        int state = YG2.GetState(IsFirstStartName);

        if (state != 1)
        {
            YG2.SetState(IsFirstStartName, 1);

            YG2.SetState(TextQuizScoreKey, 0);
            YG2.SetState(ImageQuizScoreKey, 0);
            YG2.SetState(AudioQuizScoreKey, 0);
        }
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
        int imageScore = YG2.GetState(ImageQuizScoreKey);
        int audioScore = YG2.GetState(AudioQuizScoreKey);
        int secretScore = YG2.GetState(TextQuizScoreKey);

        _imageQuizScoreText.text = SetColorScore(imageScore);
        _audioQuizScoreText.text = SetColorScore(audioScore);
        _textQuizScoreText.text = SetColorScore(secretScore);
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