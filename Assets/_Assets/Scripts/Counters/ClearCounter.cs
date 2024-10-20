using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounterScript 
{
    
    public override void Interact(IKitchenObjectParent player)
    {
        if (player.HasKitchenObject())
        {
            if (!HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenParent(this);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlatesObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else if (GetKitchenObject().TryGetPlate(out PlatesObject kitchenPlate))
                {
                    if (kitchenPlate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
        else
        {
            if (HasKitchenObject())
            {
                kitchenObject.SetKitchenParent(player);
            }
        }
    }

    
}
