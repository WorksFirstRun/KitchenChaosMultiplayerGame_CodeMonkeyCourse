using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validIngredients;
    private List<KitchenObjectSO> addedIngredients;

    public event EventHandler<VisualsShowArguments> onVisualsShow;

    public class VisualsShowArguments : EventArgs
    {
        public KitchenObjectSO KitchenObjectSo;
    }
    

    private void Start()
    {
        addedIngredients = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO ingredient)
    {
        if (addedIngredients.Contains(ingredient) || !validIngredients.Contains(ingredient))
        {
            return false;
        }

        AddIngredientsServerRpc(KitchenGameMultiPlayer.Instance.GetKitchenObjectIndex(ingredient));
        return true;
    }


    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientsServerRpc(int ingredientIndex)
    {
        AddIngredientsClientRpc(ingredientIndex);
    }
    
    [ClientRpc]
    private void AddIngredientsClientRpc(int ingredientIndex)
    {
        KitchenObjectSO ingredient = KitchenGameMultiPlayer.Instance.GetKitchenObjectSoFromIndex(ingredientIndex);
        addedIngredients.Add(ingredient);
        onVisualsShow?.Invoke(this,new VisualsShowArguments()
        {
            KitchenObjectSo = ingredient
        });
    }

    public List<KitchenObjectSO> GetAddedIngredients()
    {
        return addedIngredients;
    }

}
