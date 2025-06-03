using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPlayerPhoto : MonoBehaviour
{
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        if (_leaderboard.PlayerPhotoSprite != null)
            _image.sprite = _leaderboard.PlayerPhotoSprite;
        else
            Debug.LogError("Фото не загружено");
    }
}