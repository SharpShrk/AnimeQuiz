using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageQuestionDisplay : QuestionDisplay
{
    [SerializeField] private Image _questionImage;
    [SerializeField] private TMP_Text[] _answerText;

    protected override void Start()
    {
        base.Start(); // вызов клонирования материалов
        MetricsTracker.Instance.ReportImageQuizStart();
    }

    public override void SetQuestion(QuizQuestion question)
    {
        ShowImageQuestion(question.QuestionImage, question.Answers);
    }

    private void ShowImageQuestion(Sprite questionSprite, Answer[] answers)
    {
        ResetAnswerButtons();
        _questionImage.sprite = questionSprite;

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
        MetricsTracker.Instance.ReportImageQuizCompleted(CurrentSessionCount);
    }
}
