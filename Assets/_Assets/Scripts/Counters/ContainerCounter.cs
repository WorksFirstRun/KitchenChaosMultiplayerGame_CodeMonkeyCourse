using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ContainerCounter : BaseCounterScript
{
    [FormerlySerializedAs("kitchenObjTemplate")] [SerializeField] private KitchenObjectSO kitchenObjectObj;

    public event EventHandler OpenAnimation;
    public override void Interact(IKitchenObjectParent player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.CreateKitchenObject(kitchenObjectObj, player);
            OpenAnimationServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenAnimationServerRpc()
    {
        OpenAnimationClientRpc();
    }

    [ClientRpc]
    private void OpenAnimationClientRpc()
    {
        OpenAnimation?.Invoke(this,EventArgs.Empty);
    }
}
