using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IhasProgressBar 
{
    public event EventHandler<ProgressBarArguments> HandleProgressBar;
    public class ProgressBarArguments : EventArgs
    {
        public float progressBarFill;
    }
}
