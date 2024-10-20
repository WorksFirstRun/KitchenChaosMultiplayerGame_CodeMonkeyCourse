using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleRecipeUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform iconsContainer;
    [SerializeField] private Transform iconTemplate;

    private void Start()
    {
        iconTemplate.gameObject.SetActive(false);
    }


    public void SetRecipeText(FinalRecipeSO r)
    {
        text.text = r.recipeName;
        foreach (Transform child in iconsContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kt in r.finalRecipesList)
        {
            Transform icon = Instantiate(iconTemplate, iconsContainer);
            icon.gameObject.SetActive(true);
            icon.gameObject.GetComponent<Image>().sprite = kt.sprite;
        }
    }
}
