using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextQuizDisplay : QuestionDisplay
{
    [SerializeField] private TMP_Text _questionText;
    [SerializeField] private TMP_Text _tittleText;
    [SerializeField] private TMP_Text[] _answerText;

    private QuizQuestion _currentQuestion;

    protected override void Start()
    {
        base.Start();
        MetricsTracker.Instance.ReportTextQuizStart();
    }

    public override void SetQuestion(QuizQuestion question)
    {
        _currentQuestion = question;
        ShowTextQuestion(_currentQuestion.QuestionText, _currentQuestion.QuestionTittle, _currentQuestion.Answers);       
    }

    private void ShowTextQuestion(string questionText, string tittle , Answer[] answers)
    {
        ResetAnswerButtons();

        _tittleText.text = tittle;
        _questionText.text = questionText;

        for (int i = 0; i < answers.Length; i++)
        {
            SetAnswerButton(AnswerButtons[i], _answerText[i], answers[i]);
        }
    }

    private void SetAnswerButton(Button button, TMP_Text text, Answer answer)
    {
        text.text = answer.AnswerText;

        ResetButtonTint(button);

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => OnAnswerSelected(button, answer));
    }

    public override void ReportSessionCompleted()
    {
        if (QuizStepAlreadyReported) return;

        QuizStepAlreadyReported = true;
        MetricsTracker.Instance.ReportTextQuizCompleted(CurrentSessionCount);
    }
}
