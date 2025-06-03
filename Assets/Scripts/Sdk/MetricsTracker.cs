using System.Collections.Generic;
using UnityEngine;
using YG;

public class MetricsTracker : MonoBehaviour
{
    public static MetricsTracker Instance { get; private set; }

    private const string TextKey = "mode_text";
    private const string ImageKey = "mode_image";
    private const string AudioKey = "mode_audio";
    private const string AllModesSentKey = "modes_all_sent";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Запуск режимов

    public void ReportTextQuizStart()
    {
        YG2.MetricaSend("start_text_quiz");
        MarkModePlayed(TextKey);
    }

    public void ReportImageQuizStart()
    {
        YG2.MetricaSend("start_image_quiz");
        MarkModePlayed(ImageKey);
    }

    public void ReportAudioQuizStart()
    {
        YG2.MetricaSend("start_audio_quiz");
        MarkModePlayed(AudioKey);
    }

    //Завершение раундов с подсчётом

    public void ReportTextQuizCompleted(int sessionCount)
    {
        var data = new Dictionary<string, object> { { "count", sessionCount } };
        YG2.MetricaSend("completed_text_session", data);
    }

    public void ReportImageQuizCompleted(int sessionCount)
    {
        var data = new Dictionary<string, object> { { "count", sessionCount } };
        YG2.MetricaSend("completed_image_session", data);
    }

    public void ReportAudioQuizCompleted(int sessionCount)
    {
        var data = new Dictionary<string, object> { { "count", sessionCount } };
        YG2.MetricaSend("completed_audio_session", data);
    }

    //проверка на все три режима

    private void MarkModePlayed(string key)
    {
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        if (PlayerPrefs.GetInt(TextKey, 0) == 1 &&
            PlayerPrefs.GetInt(ImageKey, 0) == 1 &&
            PlayerPrefs.GetInt(AudioKey, 0) == 1 &&
            PlayerPrefs.GetInt(AllModesSentKey, 0) == 0)
        {
            YG2.MetricaSend("all_modes_played");
            PlayerPrefs.SetInt(AllModesSentKey, 1);
            PlayerPrefs.Save();
        }
    }
}
