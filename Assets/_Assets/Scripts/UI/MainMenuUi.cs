using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Awake()
    {
        playButton.onClick.AddListener(() => { Loader.LoadSceneMode( Loader.Scene.LobbyScene );});
        quitButton.onClick.AddListener(() => {Application.Quit();});
        Time.timeScale = 1f;
    }
    
    
}
