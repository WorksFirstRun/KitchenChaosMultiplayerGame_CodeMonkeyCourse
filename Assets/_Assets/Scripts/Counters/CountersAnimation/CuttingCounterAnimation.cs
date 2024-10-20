using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private const string CUT = "Cut";

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void Start()
    {
        cuttingCounter.OnCut += OnCutAnimation;

    }

    private void OnCutAnimation(object sender, EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
