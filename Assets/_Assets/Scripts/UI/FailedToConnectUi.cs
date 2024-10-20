
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FailedToConnectUi : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI messageText;
    
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        KitchenGameMultiPlayer.Instance.OnClientFailToConnect += PlayerFailedToConnect;
        Hide();
    }

    private void PlayerFailedToConnect()
    {
        Show();
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            messageText.text = "Check your Internet";
        }
        else
        {
            messageText.text = NetworkManager.Singleton.DisconnectReason;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiPlayer.Instance.OnClientFailToConnect -= PlayerFailedToConnect;
    }
}
