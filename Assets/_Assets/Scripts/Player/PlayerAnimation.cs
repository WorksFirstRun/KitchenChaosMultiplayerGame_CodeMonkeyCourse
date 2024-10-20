using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    private Animator playerAnimation;
    [SerializeField] private PlayerMovement player;

    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        playerAnimation = GetComponent<Animator>();
    }


    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        playerAnimation.SetBool(IS_WALKING, player.PlayerState());
    }

    
}
