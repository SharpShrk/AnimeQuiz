using UnityEngine;
using UnityEditor;
using System.IO;

public class TextQuizCSVImporter : EditorWindow
{
    [MenuItem("Tools/������������� TextQuiz �� CSV (| �����������)")]
    public static void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("�������� CSV-����", "", "csv");

        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path, System.Text.Encoding.UTF8);

        string folderPath = "Assets/QuizData";

        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets", "QuizData");

        for (int i = 1; i < lines.Length; i++) // ���������� ���������
        {
            string[] parts = lines[i].Split('|');

            if (parts.Length < 6)
            {
                Debug.LogWarning($"������ {i + 1} ����� ������������ ��������.");
                continue;
            }

            // �������� �������
            QuizQuestion question = ScriptableObject.CreateInstance<QuizQuestion>();
            question.QuestionTittle = parts[0];
            question.QuestionText = parts[1];
            question.Answers = new Answer[4];

            for (int j = 0; j < 4; j++)
            {
                question.Answers[j] = new Answer
                {
                    AnswerText = parts[2 + j],
                    IsCorrect = (j == 0), // ������ ����������
                    AnswerImage = null,
                    AnswerAudio = null
                };
            }

            string assetName = $"Question_{i}.asset";
            string assetPath = Path.Combine(folderPath, assetName);
            AssetDatabase.CreateAsset(question, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("������ �������� �������.");
    }
}
