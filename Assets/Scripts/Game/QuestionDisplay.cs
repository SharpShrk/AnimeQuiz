using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class QuestionDisplay : MonoBehaviour
{
    [SerializeField] protected Button[] AnswerButtons;
    [SerializeField] protected AudioSource AudioSource;
    [SerializeField] protected AudioClip CorrectAnswerSound;
    [SerializeField] protected AudioClip IncorrectAnswerSound;
    [SerializeField] protected ScoreManager ScoreManager;

    [SerializeField] protected Color CorrectColor = Color.green;
    [SerializeField] protected Color IncorrectColor = Color.red;

    protected int CurrentSessionCount = 0;
    protected bool QuizStepAlreadyReported = false;
    protected Color DefaultColor = Color.white;

    public event Action OnAnswered;
    public event Action OnIncorrectAnswer;

    protected virtual void Start()
    {
        // Клонируем материал на каждой кнопке
        foreach (var button in AnswerButtons)
        {
            var image = button.GetComponent<Image>();
            image.material = new Material(image.material);
        }
    }

    public abstract void SetQuestion(QuizQuestion question);

    protected void NotifyAnswered()
    {
        OnAnswered?.Invoke();
    }

    protected void DisableAllAnswerButtons()
    {
        foreach (var button in AnswerButtons)
            button.interactable = false;
    }

    protected virtual void ResetAnswerButtons()
    {
        foreach (var button in AnswerButtons)
        {
            button.interactable = true;
            ResetButtonTint(button);
        }
    }

    protected void SetButtonTint(Button button, Color color)
    {
        var image = button.GetComponent<Image>();

        if (image != null && image.material.HasProperty("_UseOverrideColor") && image.material.HasProperty("_OverrideColor"))
        {
            image.material.SetFloat("_UseOverrideColor", 1f);
            image.material.SetColor("_OverrideColor", color);
        }
    }

    protected void ResetButtonTint(Button button)
    {
        var image = button.GetComponent<Image>();

        if (image != null)
        {
            if (image.material.HasProperty("_UseOverrideColor"))
            {
                image.material.SetFloat("_UseOverrideColor", 0f);
            }

            if (image.material.HasProperty("_Tint"))
            {
                image.material.SetColor("_Tint", DefaultColor);
            }
        }
    }

    protected virtual void OnAnswerSelected(Button clickedButton, Answer answer)
    {
        if (answer.IsCorrect)
        {
            SetButtonTint(clickedButton, CorrectColor);
            AudioSource.PlayOneShot(CorrectAnswerSound);
            ScoreManager.AddPoint();
        }
        else
        {
            SetButtonTint(clickedButton, IncorrectColor);
            AudioSource.PlayOneShot(IncorrectAnswerSound);
            OnIncorrectAnswer?.Invoke();
        }

        CurrentSessionCount++;
        DisableAllAnswerButtons();
        NotifyAnswered();
    }

    public abstract void ReportSessionCompleted();
}
