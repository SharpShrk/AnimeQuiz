using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question")]
public class QuizQuestion : ScriptableObject
{
    [Header("Заголовок")]
    public string QuestionTittle;

    [Header("Вопрос (Картинка)")]
    public Sprite QuestionImage;

    [Header("Вопрос (Текст)")]
    public string QuestionText;

    [Header("Вопрос (Звук)")]
    public AudioClip QuestionAudio;

    [Header("Ответы (Текст)")]
    public Answer[] Answers;

    public Answer[] GetAnswers()
    {
        return Answers;
    }
}

[System.Serializable]
public class Answer
{
    public string AnswerText;
    public Sprite AnswerImage;
    public AudioClip AnswerAudio;
    public bool IsCorrect;
}