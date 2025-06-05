using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class ScoreManager : MonoBehaviour
{
    private const string _audioQuizScoreKey = "AudioQuizScore";
    private const string _imageQuizScoreKey = "ImageQuizScore";
    private const string _textQuizScoreKey = "TextScore";
    private const string _audioQuizHighScoreKey = "AudioQuizHighScore";
    private const string _imageQuizHighScoreKey = "ImageQuizHighScore";
    private const string _textQuizHighScoreKey = "TextQuizHighScore";
    private const string _startSceneName = "StartScene";

    [SerializeField] private GameObject _panelUI;
    [SerializeField] private GameObject _panelScore;
    [SerializeField] private GameObject _panelReview;
    [SerializeField] private GameObject[] _resultPanels;
    [SerializeField] private QuizManager _quizManager;
    [SerializeField] private Button _reviewButton;
    [SerializeField] private Button _skipReviewButton;
    [SerializeField] private Button _exitMainMenuButton;
    [SerializeField] private TMP_Text _scoreValueText;

    private int _score = 0;
    private int _questionsCount;
    private string _scoreKey;
    private string _highScoreKey;

    public void Init(QuizType quizType, int questionsCount)
    {
        switch (quizType)
        {
            case QuizType.Image:
                _scoreKey = _imageQuizScoreKey;
                _highScoreKey = _imageQuizHighScoreKey;
                break;
            case QuizType.Text:
                _scoreKey = _textQuizScoreKey;
                _highScoreKey = _textQuizHighScoreKey;
                break;
            case QuizType.Audio:
                _scoreKey = _audioQuizScoreKey;
                _highScoreKey = _audioQuizHighScoreKey;
                break;
            default:
                Debug.LogError("Неизвестный тип вопроса");
                break;
        }
        
        //_score = YG2.GetState(_scoreKey);
        _questionsCount = questionsCount;
    }

    private void OnEnable()
    {
        _panelScore.SetActive(false);
        _panelReview.SetActive(false);

        _reviewButton.onClick.AddListener(OpenYGReview);
        _skipReviewButton.onClick.AddListener(OpenScorePanel);
        _exitMainMenuButton.onClick.AddListener(OnButtonExitMenuClick);
    }

    private void OnDisable()
    {
        _reviewButton.onClick.RemoveListener(OpenYGReview);
        _skipReviewButton.onClick.RemoveListener(OpenScorePanel);
        _exitMainMenuButton.onClick.RemoveListener(OnButtonExitMenuClick);
    }

    public void AddPoint()
    {
        _score++;
        SaveScore();

        int highScore = YG2.GetState(_highScoreKey);

        if (_score > highScore)
        {
            YG2.SetState(_highScoreKey, _score);
        }
    }

    public void ResetScore()
    {
        _score = 0;
        SaveScore();
    }

    public void CheckReviewPermission()
    {
        if (YG2.reviewCanShow)
            OpenReviewPanel();
        else
            OpenScorePanel();

    }

    private void OpenReviewPanel()
    {
        _panelUI.SetActive(false);
        _panelReview.SetActive(true);
    }

    private void OpenYGReview()
    {
        YG2.ReviewShow();
    }

    private void SaveScore()
    {
        YG2.SetState(_scoreKey, _score);
        ScoreSaver.Instance.TrySaveHighScore();
    }

    private void OpenScorePanel()
    {
        int result = 0;

        float scorePercent = ((float)_score / _questionsCount) * 100f;

        if (scorePercent <= 30)
            result = 0;
        else if (scorePercent <= 60)
            result = 1;
        else if (scorePercent <= 80)
            result = 2;
        else
            result = 3;

        _panelUI.SetActive(false);
        _panelReview.SetActive(false);
        _panelScore.SetActive(true);
        _resultPanels[result].SetActive(true);
        SetTextResult(_score);
    }

    private void OnButtonExitMenuClick()
    {
        SaveScore();
        SceneManager.LoadScene(_startSceneName);
    }

    private void SetTextResult(int score)
    {
        string coloredScore = $"<color=#E9BA00><b>{score}</b></color>";
        string result;

        if (_quizManager.IsRu)
        {
            result = $"Вы набрали {coloredScore} балла!";
            _scoreValueText.text = result;
        }
        else
        {
            result = $"You scored {coloredScore} points!";
            _scoreValueText.text = result;
        }
    }
}
