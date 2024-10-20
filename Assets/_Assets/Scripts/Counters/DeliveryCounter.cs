using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounterScript
{
    public static DeliveryCounter Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public override void Interact(IKitchenObjectParent player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlatesObject plate))
            {
                DeliveryManager.Instance.Deliver_a_Recipe(plate.GetAddedIngredients());
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
