using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    private IhasProgressBar counter;
    [SerializeField] private GameObject iHasProgressBar;

    private void Start()
    {
        if (iHasProgressBar.TryGetComponent<IhasProgressBar>(out IhasProgressBar c))
        {
            counter = c;
            counter.HandleProgressBar += CounterOnHandleProgressBar;
            barImage.fillAmount = 0f;
            Hide();
        }
        else
        {
            Debug.LogError("Game object" + iHasProgressBar + "Doesnt have a IhasProgressBar Interface");
        }
        
    }

    private void CounterOnHandleProgressBar(object sender, IhasProgressBar.ProgressBarArguments e)
    {
        barImage.fillAmount = e.progressBarFill;
        if (e.progressBarFill == 0f || e.progressBarFill == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    
}
