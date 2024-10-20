using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisualSelection : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyText;
    [SerializeField] private PlayerColorAndVisual _playerColorAndVisual;
    
    private void Start()
    {
        KitchenGameMultiPlayer.Instance.OnPlayerDataListChanged += InstanceOnOnPlayerDataListChanged;
        KitchenGameMultiPlayer.Instance.OnPlayerIsReady += SomeOneIsReady;
        UpdateVisuals();
        
    }
    
    private void SomeOneIsReady()
    {
       UpdateVisuals();
    }

   

    private void InstanceOnOnPlayerDataListChanged()
    {
        UpdateVisuals();
    }


    private void UpdateVisuals()
    {
        if (KitchenGameMultiPlayer.Instance.isPlayerConnected(playerIndex))
        {
            Show();
            PlayerData playerData = KitchenGameMultiPlayer.Instance.GetPlayerDataFromIndex(playerIndex);
            readyText.SetActive(KitchenGameMultiPlayer.Instance.isPlayerReady(playerData.clientid));
            _playerColorAndVisual.SetPlayerColor(KitchenGameMultiPlayer.Instance.GetPlayerColor(playerData.colorid));
        }
        else
        {
            Hide();
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
        KitchenGameMultiPlayer.Instance.OnPlayerDataListChanged -= InstanceOnOnPlayerDataListChanged;
        KitchenGameMultiPlayer.Instance.OnPlayerIsReady -= SomeOneIsReady;
    }
}
