using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedColor : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image ButtonColorImage;
    [SerializeField] private GameObject selectedVisual;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenGameMultiPlayer.Instance.ChangePlayerColor(colorId);
        });
    }

    private void Start()
    {
        KitchenGameMultiPlayer.Instance.OnPlayerDataListChanged += PlayerDataListChanged;
        ButtonColorImage.color = KitchenGameMultiPlayer.Instance.GetPlayerColor(colorId);
        UpdateColor();
    }

    private void PlayerDataListChanged()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (KitchenGameMultiPlayer.Instance.GetPlayerData().colorid == colorId)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        KitchenGameMultiPlayer.Instance.OnPlayerDataListChanged -= PlayerDataListChanged;
    }
}
