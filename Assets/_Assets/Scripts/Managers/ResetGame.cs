using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.ResetEvents();
        TrashScript.ResetEvents();
        BaseCounterScript.ResetEvents();
        PlayerMovement.ResetStaticData();
    }
}
