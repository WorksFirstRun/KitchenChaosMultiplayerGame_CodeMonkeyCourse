
using System;
using UnityEngine;

public class ConnectingUi : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiPlayer.Instance.OnClientTryToConnect += PlayerConnecting;
        KitchenGameMultiPlayer.Instance.OnClientFailToConnect += PlayerFailedToConnect;
        Hide();
    }

    private void PlayerFailedToConnect()
    {
        Hide();
    }

    private void PlayerConnecting()
    {
        Show();
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
        KitchenGameMultiPlayer.Instance.OnClientTryToConnect -= PlayerConnecting;
        KitchenGameMultiPlayer.Instance.OnClientFailToConnect -= PlayerFailedToConnect;
    }
}
