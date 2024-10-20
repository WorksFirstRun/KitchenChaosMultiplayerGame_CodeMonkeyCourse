using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : NetworkBehaviour
{
    private IKitchenObjectParent kitchenObjectParent; 
    [SerializeField] private KitchenObjectSO kitchenObject;
    private FollowTransform followTransform;

    private void Awake()
    {
        followTransform = gameObject.GetComponent<FollowTransform>();
    }

    public void SetKitchenParent(IKitchenObjectParent kitchenObjectParent)
    {
       SetKitchenParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObject)
    {
        SetKitchenParentClientRpc(kitchenObjectParentNetworkObject);
    }
    
    [ClientRpc]
    private void SetKitchenParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObject)
    {
        kitchenObjectParentNetworkObject.TryGet(out NetworkObject NB_kitchenObjectParent);
        IKitchenObjectParent kitchenObjectParent = NB_kitchenObjectParent.GetComponent<IKitchenObjectParent>();
        
        
        if (kitchenObjectParent.HasKitchenObject()) 
        {
            Debug.Log("Already Has one");
            return;
        }

        if (this.kitchenObjectParent != null) // if it has been set before , so change it 
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        
        this.kitchenObjectParent = kitchenObjectParent;
        this.kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetTargetTransform(this.kitchenObjectParent.GetParentHoldPoint());
    }

    public IKitchenObjectParent GetKitchenParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        DestroyKitchenObject(NetworkObject);
    }

    public void DestroySelfServerSide()
    {
        Destroy(gameObject);
    }

    public static void CreateKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiPlayer.Instance.CreateKitchenObject(kitchenObjectSo,kitchenObjectParent);
    }

    private void DestroyKitchenObject(NetworkObjectReference kitchenObjectNo)
    {
        KitchenGameMultiPlayer.Instance.DestroyKitchenObject(kitchenObjectNo);
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObject;
    }

    public bool TryGetPlate(out PlatesObject plate)
    {
        if (this is PlatesObject)
        {
            plate = this as PlatesObject;
            return true;
        }
        else
        {
            plate = null;
            return false;
        }
    }

}
