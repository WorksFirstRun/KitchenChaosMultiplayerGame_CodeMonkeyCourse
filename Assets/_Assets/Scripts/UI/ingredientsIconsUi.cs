using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredientsIconsUi : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private PlatesObject plate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plate.onVisualsShow += PlateOnVisualsShow;
    }

    private void PlateOnVisualsShow(object sender, PlatesObject.VisualsShowArguments e)
    {
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kt in plate.GetAddedIngredients())
        {
            Transform icon = Instantiate(iconTemplate,transform);
            icon.gameObject.SetActive(true);
            icon.GetComponent<IconScriptUi>().ChangeSprite(kt);
            
            
        }
    }
}
