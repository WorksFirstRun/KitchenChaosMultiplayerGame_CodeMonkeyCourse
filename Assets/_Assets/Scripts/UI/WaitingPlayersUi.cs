using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPlayersUi : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button readyButton;
    private bool toggleUi;
    private Color smoothGreen = new Color(0f / 255f, 204f / 255f, 102f / 255f);

    public event Action<bool> OnPlayerToggleReady;
    
    private void Start()
    {
        readyButton.onClick.AddListener(ToggleReady);
    }

    private void ToggleReady()
    {
        toggleUi = !toggleUi;
        if (toggleUi)
        {
            buttonImage.color = smoothGreen;
            OnPlayerToggleReady?.Invoke(toggleUi);
        }
        else
        {
            buttonImage.color = Color.white;
            OnPlayerToggleReady?.Invoke(toggleUi);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
