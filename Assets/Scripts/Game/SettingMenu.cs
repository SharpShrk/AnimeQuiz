using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    private const string SFX_KEY = "SoundFXVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string _startSceneName = "StartScene";

    [Header("UI")]
    [SerializeField] private QuestionDisplay _questionDisplay;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitSettingsButton;
    [SerializeField] private Button _exitMainMenu;
    [SerializeField] private GameObject _canvasUI;
    [SerializeField] private GameObject _canvasSettins;

    [Header("AudioSettings")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private float _startSoundEffectVolume = 0.3f;
    [SerializeField] private float _startMusicVolume = 0.05f;

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _exitSettingsButton.onClick.AddListener(OnExitButtonClick);
        _exitMainMenu.onClick.AddListener(ExitMainMenu);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        _exitSettingsButton.onClick.RemoveListener(OnExitButtonClick);
        _exitMainMenu.onClick.RemoveListener(ExitMainMenu);
    }

    private void Start()
    {
        HideSettinsPanel();
        SetVolumeOnStart();
    }

    private void OnSettingsButtonClick()
    {
        OpenSettingsPanel();
    }

    private void OnExitButtonClick()
    {
        HideSettinsPanel();
    }

    private void HideSettinsPanel()
    {
        _canvasUI.SetActive(true);
        _canvasSettins.SetActive(false);
    }

    private void OpenSettingsPanel()
    {
        _canvasUI.SetActive(false);
        _canvasSettins.SetActive(true);
    }

    private void SetVolumeOnStart()
    {
        float sfxValue = PlayerPrefs.GetFloat(SFX_KEY, _startSoundEffectVolume);
        _sfxSlider.value = sfxValue;
        SetSFXVolume(sfxValue);

        if (_questionDisplay is AudioQuestionDisplay)
        {
            _musicSlider.value = 0f;
            _audioMixer.SetFloat(MUSIC_KEY, Mathf.Log10(0.0001f) * 20);
        }
        else
        {
            float musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, _startMusicVolume);
            _musicSlider.value = musicValue;
            SetMusicVolume(musicValue);
        }

        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void SetSFXVolume(float value)
    {
        _audioMixer.SetFloat(SFX_KEY, Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat(MUSIC_KEY, Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    private void ExitMainMenu()
    {
        _questionDisplay.ReportSessionCompleted();
        SceneManager.LoadScene(_startSceneName);
    }

    private void OnDestroy()
    {
        _sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }
}
