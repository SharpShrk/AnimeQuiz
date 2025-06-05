using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PaymentManager : MonoBehaviour
{
    private const string PurchaseStatusName = "PurchaseStatus";
    private const string AdsPurchaseID = "ads";

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _marketButton;
    [SerializeField] private GameObject _marketUI;
    [SerializeField] private GameObject _gameUI;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(HideMarket);
        _marketButton.onClick.AddListener(OpenMarket);
        YG2.onPurchaseSuccess += SuccessPurchased;       
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(HideMarket);
        _marketButton.onClick.RemoveListener(OpenMarket);
        YG2.onPurchaseSuccess -= SuccessPurchased;
    }

    private void Start()
    {
        if (YG2.GetState(PurchaseStatusName) == 1)
        {
            _marketButton.interactable = false;
        }
    }

    private void OpenMarket()
    {
        _gameUI.SetActive(false);
        _marketUI.SetActive(true);
    }

    private void HideMarket()
    {
        _gameUI.SetActive(true);
        _marketUI.SetActive(false);
    }

    private void SuccessPurchased(string id)
    {
        if (id == AdsPurchaseID)
        {
            ADManager.Instance.DisableInterstitialAds();
            YG2.SetState(PurchaseStatusName, 1);
            HideMarket();
            _marketButton.gameObject.SetActive(false);
        }
    }
}