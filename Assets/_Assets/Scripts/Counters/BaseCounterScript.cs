using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounterScript : NetworkBehaviour , IKitchenObjectParent
{
    [SerializeField] protected Transform counterTopPoint;
    public static event EventHandler onPlayObjectDropSound;

    public static void ResetEvents()
    {
        onPlayObjectDropSound = null;
    }
    
    protected KitchenObject kitchenObject;
    
    public virtual void Interact(IKitchenObjectParent kitchenObjectParent)
    {
    }

    public virtual void CutInteraction()
    {
    }
    
    public Transform GetParentHoldPoint()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
        if (kitchenObject != null)
        {
            onPlayObjectDropSound?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
