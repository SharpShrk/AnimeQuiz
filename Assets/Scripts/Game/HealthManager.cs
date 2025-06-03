using UnityEngine;
using UnityEngine.UI;
using YG;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private QuestionDisplay _questionDisplay;
    [SerializeField] private QuizManager _quizManager;
    [SerializeField] private GameObject[] _heartsImage;
    [SerializeField] private GameObject[] _otherCanvases;
    [SerializeField] private GameObject _advMessagePanel;
    [SerializeField] private Button _advButton;
    [SerializeField] private Button _cancelButton;

    private int _maxHealth = 5;
    private int _health;
    private string _rewardID = "health";

    private void OnEnable()
    {
        _questionDisplay.OnIncorrectAnswer += SubtractHealth;
        _advButton.onClick.AddListener(RewardAdvShow);
        _cancelButton.onClick.AddListener(GameOver);
        HideAdvMessagePanel();
        _health = _maxHealth;
    }

    private void OnDisable()
    {
        _questionDisplay.OnIncorrectAnswer -= SubtractHealth;
        _advButton.onClick.RemoveListener(RewardAdvShow);
        _cancelButton.onClick.RemoveListener(GameOver);
    }

    private void SubtractHealth()
    {
        if (_health > 0)
        {
            _health--;
            UpdateHearts();
        }

        if (_health == 0)
        {
            ShowAdvMessagePanel();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < _heartsImage.Length; i++)
        {
            _heartsImage[i].SetActive(i < _health);
        }
    }

    private void ShowAdvMessagePanel()
    {
        foreach( var obj  in _otherCanvases )
            obj.SetActive(false);

        _advMessagePanel.SetActive(true);
    }

    private void HideAdvMessagePanel()
    {
        foreach (var obj in _otherCanvases)
            obj.SetActive(true);

        _advMessagePanel.SetActive(false);
    }

    private void RewardAdvShow()
    {
        YG2.RewardedAdvShow(_rewardID, () =>
        {
            if (_rewardID == "health")
            {
                _health = _maxHealth;
                UpdateHearts();
                HideAdvMessagePanel();
            }
        });
    }

    private void GameOver()
    {
        HideAdvMessagePanel();
        _quizManager.GameOver();
    }
}
