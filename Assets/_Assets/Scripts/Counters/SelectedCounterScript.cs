using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterScript : MonoBehaviour
{
    [SerializeField] private BaseCounterScript counter;
    [SerializeField] private GameObject[] selectedVisual;
    
    private void Start()
    {
        if (PlayerMovement.LocalInstance != null)
        {
            PlayerMovement.LocalInstance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
        }
        else
        {
            PlayerMovement.onPlayerSpawn += PlayerSpawnAttachLocalInstance;
        }
    }

    private void PlayerSpawnAttachLocalInstance(object sender, EventArgs e)
    {
        if (PlayerMovement.LocalInstance != null)
        {
           PlayerMovement.LocalInstance.OnSelectedCounterChange -= Player_OnSelectedCounterChange;
           PlayerMovement.LocalInstance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
        }
    }

    private void Player_OnSelectedCounterChange(object sender, PlayerMovement.OnSelectedCounterChangeArgs e)
    {
        if (e.selectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        foreach (var g in selectedVisual)
        {
            g.SetActive(false);
        }
        
    }

    private void Show()
    {
        foreach (var g in selectedVisual)
        {
            g.SetActive(true);
        }
    }
}
