using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question")]
public class QuizQuestion : ScriptableObject
{
    [Header("���������")]
    public string QuestionTittle;

    [Header("������ (��������)")]
    public Sprite QuestionImage;

    [Header("������ (�����)")]
    public string QuestionText;

    [Header("������ (����)")]
    public AudioClip QuestionAudio;

    [Header("������ (�����)")]
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