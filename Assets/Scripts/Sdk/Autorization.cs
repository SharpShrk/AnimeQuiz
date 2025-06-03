using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Autorization : MonoBehaviour
{
    [SerializeField] private GameObject _autorizationPanel;
    [SerializeField] private GameObject _otherUI;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _autorizationButton;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(HideAutorizationPanel);
        _autorizationButton.onClick.AddListener(CallAutorizationYG);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(HideAutorizationPanel);
        _autorizationButton.onClick.RemoveListener(CallAutorizationYG);
    }

    public void OpenAutorizationPanel()
    {
        _otherUI.SetActive(false);
        _autorizationPanel.SetActive(true);
    }

    private void HideAutorizationPanel()
    {
        _autorizationPanel.SetActive(false);
        _otherUI.SetActive(true);
    }

    private void CallAutorizationYG()
    {
        YG2.OpenAuthDialog();
        HideAutorizationPanel();
    }
}
