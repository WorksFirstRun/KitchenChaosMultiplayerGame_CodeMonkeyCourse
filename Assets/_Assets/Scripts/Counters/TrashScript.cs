using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : BaseCounterScript
{
   public static event EventHandler onTrashSoundPlay;

   public new static void ResetEvents()
   {
      onTrashSoundPlay = null;
   }
   
   public override void Interact(IKitchenObjectParent player)
   {
      if (player.HasKitchenObject())
      {
         onTrashSoundPlay?.Invoke(this,EventArgs.Empty);
         player.GetKitchenObject().DestroySelf();
      }
   }
}
