using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


public class ProgressBarLookAt : MonoBehaviour
{
    private enum Mode
    {
        CameraForward,
        CameraForwardInvered
    }

    [SerializeField] private Mode CameraMode;

    private void LateUpdate()
    {
        switch(CameraMode)
        {
            case Mode.CameraForward:
            transform.forward = Camera.main.transform.forward;
            break;
            case Mode.CameraForwardInvered:
            transform.forward = -Camera.main.transform.forward;
            break;
        }
    }
}
