using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUi : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;


    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.onRecipeVisualShow += InstanceOnonRecipeVisualShow;
        UpdateVisuals();
    }

    private void InstanceOnonRecipeVisualShow(object sender, EventArgs e)
    {
        UpdateVisuals();
    }


    private void UpdateVisuals()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (FinalRecipeSO r in DeliveryManager.Instance.GetWaitingRecipes())
        {
            Transform recipeT = Instantiate(recipeTemplate, container);
            recipeT.gameObject.SetActive(true);
            if (recipeT.gameObject.TryGetComponent<SingleRecipeUi>(out SingleRecipeUi test))
            {
                test.SetRecipeText(r);
            }
        }
    }
    
}
