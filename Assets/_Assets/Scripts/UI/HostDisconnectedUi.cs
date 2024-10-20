using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class HostDisconnectedUi : MonoBehaviour
{

    [SerializeField] private Button quitGameButton;
    
    private void Start()
    {
        
        NetworkManager.Singleton.OnClientDisconnectCallback += OnHostDisconnect;
        quitGameButton.onClick.AddListener(() =>
        { 
            NetworkManager.Singleton.Shutdown();
            Loader.LoadSceneMode(Loader.Scene.MainMenu);
        });
        Hide();
    }

    private void OnHostDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            Show();
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
}
