using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button ResumButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button OptionsButton;
    
    
    
    private void Awake()
    {
        ResumButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseGame();
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadSceneMode(Loader.Scene.MainMenu);
        });
        OptionsButton.onClick.AddListener(() =>
        {
           GameManager.Instance.PauseGame();
           Time.timeScale = 0f;
           OptionsUI.Instance.Show();
        });
    }

    private void Start()
    {
        GameManager.Instance.onGamePause += InstanceOnonGamePause;
        GameManager.Instance.onGameUnPause += InstanceOnonGameUnPause;
        
        Hide();
    }

    private void InstanceOnonGameUnPause(object sender, EventArgs e)
    {
        Hide();
    }

    private void InstanceOnonGamePause(object sender, EventArgs e)
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
    
}
