using UnityEngine;
using UnityEngine.UI;
using YG;

public class LeaderboardRankPosition : MonoBehaviour
{
    [SerializeField] private Sprite[] _rankSprite;
    [SerializeField] private Image _rankImage;
    [SerializeField] private Text _rankText;

    private LBPlayerDataYG _playerData;
    private int _rank;

    private void OnEnable()
    {
        _rankImage.gameObject.SetActive(false);
        _playerData = GetComponent<LBPlayerDataYG>();

        _rank = int.Parse(_playerData.textLegasy.rank.text.ToString());

        SetRankImage();
    }

    public void SetRankImage()
    {
        if (_rank > _rankSprite.Length)
            return;

        _rankText.gameObject.SetActive(false);
        _rankImage.gameObject.SetActive(true);
        _rankImage.sprite = _rankSprite[_rank - 1];
    }
}
