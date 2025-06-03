using UnityEngine;
using UnityEditor;
using System.IO;

public class MediaQuizCSVImporter : EditorWindow
{
    [MenuItem("Tools/Импортировать MediaQuiz из CSV (| разделитель)")]
    public static void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("Выберите CSV-файл", "", "csv");

        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path, System.Text.Encoding.UTF8);

        string folderPath = "Assets/QuizData";

        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets", "QuizData");

        for (int i = 1; i < lines.Length; i++) // Пропускаем заголовок
        {
            string[] parts = lines[i].Split('|');

            if (parts.Length < 4)
            {
                Debug.LogWarning($"Строка {i + 1} имеет недостаточно столбцов.");
                continue;
            }

            // Создание объекта
            QuizQuestion question = ScriptableObject.CreateInstance<QuizQuestion>();
            question.QuestionTittle = null;
            question.QuestionText = null;
            question.Answers = new Answer[4];

            for (int j = 0; j < 4; j++)
            {
                question.Answers[j] = new Answer
                {
                    AnswerText = parts[j],
                    IsCorrect = (j == 0), // Первый правильный
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
        Debug.Log("Импорт завершен успешно.");
    }
}
