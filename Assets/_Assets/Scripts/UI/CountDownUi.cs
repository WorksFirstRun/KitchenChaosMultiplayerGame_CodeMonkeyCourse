using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountDownText;

    private void Start()
    {
        GameManager.Instance.onStateChanged += GameManager_onStateChanged;
        Hide();
    }
    

    private void GameManager_onStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameCountDownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        CountDownText.text = Mathf.Ceil(GameManager.Instance.GetCountDownTimer()).ToString();
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
