using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AudioSpectrumExporter : EditorWindow
{
    private DefaultAsset folder;
    private int fftSize = 512;
    private int stepMilliseconds = 50;
    private FFTWindow fftWindow = FFTWindow.Blackman;

    [MenuItem("Tools/Batch Audio Spectrum Exporter")]
    public static void ShowWindow()
    {
        GetWindow<AudioSpectrumExporter>("Batch Spectrum Exporter");
    }

    private void OnGUI()
    {
        folder = (DefaultAsset)EditorGUILayout.ObjectField("Audio Folder", folder, typeof(DefaultAsset), false);
        fftSize = EditorGUILayout.IntField("FFT Size", fftSize);
        stepMilliseconds = EditorGUILayout.IntField("Step (ms)", stepMilliseconds);
        fftWindow = (FFTWindow)EditorGUILayout.EnumPopup("FFT Window", fftWindow);

        if (GUILayout.Button("Export All Spectra"))
        {
            if (folder == null)
            {
                Debug.LogError("Please assign a folder with AudioClips.");
                return;
            }

            string folderPath = AssetDatabase.GetAssetPath(folder);
            ExportAllSpectra(folderPath);
        }
    }

    private void ExportAllSpectra(string folderPath)
    {
        string[] audioGuids = AssetDatabase.FindAssets("t:AudioClip", new[] { folderPath });

        string exportFolder = EditorUtility.OpenFolderPanel("Select Folder to Save JSON", "", "");
        if (string.IsNullOrEmpty(exportFolder)) return;

        foreach (string guid in audioGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogWarning($"Skipped invalid AudioClip at {path}");
                continue;
            }

            Debug.Log($"Processing: {clip.name}");
            ExportSpectrum(clip, exportFolder);
        }

        Debug.Log("Spectrum export complete.");
    }

    private void ExportSpectrum(AudioClip clip, string exportFolder)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        int channels = clip.channels;
        int totalSamples = samples.Length / channels;
        int stepSamples = Mathf.CeilToInt((stepMilliseconds / 1000f) * clip.frequency);
        int spectrumCount = totalSamples / stepSamples;

        List<float[]> spectrumOverTime = new List<float[]>();
        float[] segment = new float[fftSize];

        for (int i = 0; i < spectrumCount; i++)
        {
            int start = i * stepSamples;
            if (start + fftSize >= totalSamples) break;

            for (int j = 0; j < fftSize; j++)
            {
                segment[j] = samples[(start + j) * channels];
            }

            AudioClip tempClip = AudioClip.Create("temp", fftSize, 1, clip.frequency, false);
            tempClip.SetData(segment, 0);

            float[] spectrum = new float[fftSize];
            AudioUtility.GetSpectrum(tempClip, spectrum, fftWindow);

            spectrumOverTime.Add(spectrum);
            Object.DestroyImmediate(tempClip);
        }

        string json = JsonHelper.ToJson(spectrumOverTime.ToArray(), true);
        string savePath = Path.Combine(exportFolder, clip.name + "_spectrum.json");
        File.WriteAllText(savePath, json);
    }

    static class AudioUtility
    {
        public static void GetSpectrum(AudioClip clip, float[] spectrum, FFTWindow window)
        {
            var go = new GameObject("TempAudioSource");
            var audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();

            for (int i = 0; i < 3; i++) audioSource.GetSpectrumData(spectrum, 0, window);
            Object.DestroyImmediate(go);
        }
    }

    public static class JsonHelper
    {
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = array };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T> { public T[] Items; }
    }
}