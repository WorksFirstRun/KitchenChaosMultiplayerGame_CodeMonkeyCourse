using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerClock : MonoBehaviour
{
    [SerializeField] private Image Timer;

    private void Update()
    {
        Timer.fillAmount = GameManager.Instance.GetGameRunningTimerNormalized();
    }
}
