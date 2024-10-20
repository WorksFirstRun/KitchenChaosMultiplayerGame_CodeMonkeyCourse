using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public static event EventHandler onPlayFootSounds;
    private float soundTimer;
    private float soundTimerMax = 0.1f;
    
    private void Update()
    {
        soundTimer += Time.deltaTime;
        if (soundTimer > soundTimerMax)
        {
            soundTimer = 0;
            if (PlayerMovement.LocalInstance.PlayerState())
            {
                onPlayFootSounds?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
