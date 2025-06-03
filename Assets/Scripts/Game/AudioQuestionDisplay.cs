using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioQuestionDisplay : QuestionDisplay
{
    [SerializeField] private TMP_Text[] _answerText;
    [SerializeField] private Button _playAudioButton;

    private QuizQuestion _currentAudioQuestion;

    private void OnEnable()
    {
        _playAudioButton.onClick.AddListener(PlayAudioQuestion);
    }

    private void OnDisable()
    {
        _playAudioButton.onClick.RemoveListener(PlayAudioQuestion);
    }

    protected override void Start()
    {
        base.Start();
        MetricsTracker.Instance.ReportAudioQuizStart();
    }

    public override void SetQuestion(QuizQuestion question)
    {
        _currentAudioQuestion = question;

        ResetAnswerButtons();

        var answers = _currentAudioQuestion.Answers;

        for (int i = 0; i < answers.Length; i++)
        {
            SetAnswerButton(AnswerButtons[i], _answerText[i], answers[i]);
        }
    }

    private void PlayAudioQuestion()
    {
        AudioSource.clip = _currentAudioQuestion.QuestionAudio;
        AudioSource.loop = false;
        AudioSource.Play();
    }

    private void SetAnswerButton(Button button, TMP_Text text, Answer answer)
    {
        text.text = answer.AnswerText;
        ResetButtonTint(button);

        if (AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnAnswerSelected(button, answer));
    }

    public override void ReportSessionCompleted()
    {
        if (QuizStepAlreadyReported) return;

        QuizStepAlreadyReported = true;
        MetricsTracker.Instance.ReportAudioQuizCompleted(CurrentSessionCount);
    }
}