using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audiosource;

    private void Awake()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.onAnimationAction += StoveCounterOnonAnimationAction;
    }

    private void StoveCounterOnonAnimationAction(object sender, StoveCounter.StoveState e)
    {
        bool State = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (State)
        {
            audiosource.Play();
        }
        else
        {
            audiosource.Pause();
        }
    }
}
