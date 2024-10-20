using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void Start()
    {
        containerCounter.OpenAnimation += ContainerCounterOnOpenAnimation;
    }

    private void ContainerCounterOnOpenAnimation(object sender, EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
