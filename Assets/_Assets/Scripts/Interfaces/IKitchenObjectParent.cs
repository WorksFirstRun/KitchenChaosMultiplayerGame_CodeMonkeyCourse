using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetParentHoldPoint();
    
    public void SetKitchenObject(KitchenObject newKitchenObject);
    
    public KitchenObject GetKitchenObject();

    public bool HasKitchenObject();

    public void ClearKitchenObject();

    public NetworkObject GetNetworkObject();

}
