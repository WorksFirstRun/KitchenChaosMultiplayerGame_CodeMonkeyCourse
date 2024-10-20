using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesNumberText;
    
    private void Start()
    {
        GameManager.Instance.onStateChanged += GameManager_onStateChanged;
        Hide();
    }

    private void GameManager_onStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            recipesNumberText.text = DeliveryManager.Instance.successfulDelivers.ToString();
            Show();
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

}
