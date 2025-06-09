using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class QuizManager : MonoBehaviour
{
    private static readonly string[] _ruLocale = { "ru", "be", "kk", "uk", "uz" };

    [SerializeField] private List<QuizQuestion> _questionsRu;
    [SerializeField] private List<QuizQuestion> _questionsEn;
    [SerializeField] private QuestionDisplay _questionDisplay;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private QuizType _quizType;
    [SerializeField] private float _delayAfterAnswer = 2f;

    private List<QuizQuestion> _currentQuestions;
    private WaitForSeconds _waitForSeconds;
    private int _currentQuestionIndex = 0;
    private int _maxQuestionsNumber;
    private int _advFrequency;
    private bool _isAnswering = false;
    private bool _isRu;

    public bool IsRu => _isRu;
    public QuizType Type => _quizType;

    private void Start()
    {
        SetLanguageQuestion();

        _scoreManager.Init(_quizType, _currentQuestions.Count);

        _waitForSeconds = new WaitForSeconds(_delayAfterAnswer);
        _maxQuestionsNumber = _currentQuestions.Count;
        _progressSlider.maxValue = _currentQuestions.Count;
        _progressSlider.value = 0;

        _advFrequency = GetAdvFlag();

        ShuffleQA();
        ShowQuestion();
    }

    private void SetLanguageQuestion()
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

        _currentQuestions = _isRu ? _questionsRu : _questionsEn;
    }

    private void ShowQuestion()
    {
        if (_currentQuestionIndex < _maxQuestionsNumber)
        {
            _isAnswering = true;

            _questionDisplay.SetQuestion(_currentQuestions[_currentQuestionIndex]);

            _questionDisplay.OnAnswered += HandleAnswerSelected;
        }
        else
        {
            GameOver();
        }
    }

    private void ShuffleQA()
    {
        Shuffle(_currentQuestions);

        foreach (var question in _currentQuestions)
        {
            var answers = question.GetAnswers();
            Shuffle(answers);
        }
    }

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void HandleAnswerSelected()
    {
        if (!_isAnswering) return;

        _isAnswering = false;

        _questionDisplay.OnAnswered -= HandleAnswerSelected;

        if (_currentQuestionIndex % _advFrequency == 0 && _currentQuestionIndex != 0)
            ADManager.Instance.TryShowInterestitalAD();

        StartCoroutine(WaitAndNextQuestion());
    }

    private int GetAdvFlag()
    {
        int value = 10;

        if (YG2.TryGetFlagAsInt("advFrequency", out int advFrequency))
        {
            value = advFrequency;
        }

        return value;
    }

    private IEnumerator WaitAndNextQuestion()
    {
        yield return _waitForSeconds;

        _currentQuestionIndex++;
        _progressSlider.value = _currentQuestionIndex;

        ShowQuestion();
    }

    public void GameOver()
    {
        _questionDisplay.ReportSessionCompleted();
        _scoreManager.CheckReviewPermission();
    }
}