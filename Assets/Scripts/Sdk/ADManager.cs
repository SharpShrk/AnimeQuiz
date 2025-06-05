using System.Collections;
using UnityEngine;
using YG;

public class ADManager : MonoBehaviour
{
    public static ADManager Instance { get; private set; }

    private const string DisableADsStateSave = "DisableADsState";

    private WaitForSeconds _adsTimer;
    private bool _isCanShowInterAD;
    private bool _isInterAdsDisabled;
    private float _timerBetweenAD = 61f;

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

    private void Start()
    {
        _adsTimer = new WaitForSeconds(_timerBetweenAD);
        _isCanShowInterAD = true;
        CheckDisableInterstitialAds();
    }

    public void TryShowInterestitalAD()
    {
        if (_isInterAdsDisabled)
            return;

        if (!_isCanShowInterAD)
            return;

        ShowInterestitalAD();
    }

    public void DisableInterstitialAds()
    {
        YG2.SetState(DisableADsStateSave, 1);
        _isInterAdsDisabled = true;
    }

    private void CheckDisableInterstitialAds()
    {
        int disableAdsState = YG2.GetState(DisableADsStateSave);

        if (disableAdsState == 1)
            _isInterAdsDisabled = true;
        else
            _isInterAdsDisabled = false;
    }

    private void ShowInterestitalAD()
    {
        YG2.InterstitialAdvShow();
        _isCanShowInterAD = false;
        StartCoroutine(AdCooldownCoroutine());
    }

    private IEnumerator AdCooldownCoroutine()
    {
        yield return _adsTimer;
        _isCanShowInterAD = true;
    }
}