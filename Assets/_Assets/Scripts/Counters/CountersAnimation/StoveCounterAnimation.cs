using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterAnimation : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject cookingAnimation;

    private void Start()
    {
        stoveCounter.onAnimationAction += StoveCounterOnonAnimationAction;
    }

    private void StoveCounterOnonAnimationAction(object sender, StoveCounter.StoveState e)
    {
        bool showState = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        particles.SetActive(showState);
        cookingAnimation.SetActive(showState);
    }

    
}
