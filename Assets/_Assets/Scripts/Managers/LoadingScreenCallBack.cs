using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenCallBack : MonoBehaviour
{
    private bool isFirstUpdate = false;

    private void Update()
    {
        if (!isFirstUpdate)
        {
            isFirstUpdate = true;
            Loader.LoaderCallBack();
        }
    }
}
