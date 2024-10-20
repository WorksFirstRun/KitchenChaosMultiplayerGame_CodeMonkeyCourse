using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounterScript
{
   public event EventHandler onPlatesVisualSpawn;
   public event EventHandler onPlatesVisualRemove;
   [SerializeField] private KitchenObjectSO plates;
   private float plateSpawningTimer = 4f;
   private float spawningTimer;
   private int platesAmount;
   private int maxPlatesAmount = 4;

   private void Update()
   {
      if (!IsServer)
      {
         return;
      }
      
      spawningTimer += Time.deltaTime;
      if (spawningTimer >= plateSpawningTimer)
      {
         spawningTimer = 0;

         if (platesAmount < maxPlatesAmount)
         {
            SpawnPlatesVisualServerRpc();
         }
      }
   }

   [ServerRpc]
   private void SpawnPlatesVisualServerRpc()
   {
      SpawnPlatesVisualClientRpc();
   }

   [ClientRpc]
   private void SpawnPlatesVisualClientRpc()
   {
      platesAmount++;
      onPlatesVisualSpawn?.Invoke(this, EventArgs.Empty);
   }


   public override void Interact(IKitchenObjectParent player)
   {
      if (!player.HasKitchenObject())
      {
         if (platesAmount > 0)
         {
            KitchenObject.CreateKitchenObject(plates, player);
            InteractServerRpc();
         }
      }
   }

   [ServerRpc (RequireOwnership = false)]
   private void InteractServerRpc()
   {
      InteractClientRpc();
   }

   [ClientRpc]
   private void InteractClientRpc()
   {
      if (platesAmount < 0)
      {
         return;
      }
      platesAmount--;
      onPlatesVisualRemove?.Invoke(this,EventArgs.Empty);
   }
}
