using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounterScript , IhasProgressBar
{
    [SerializeField] private KitchenRecipeSoTemplate[] Recipes;
    private int cuttingProgress;

    public event EventHandler<IhasProgressBar.ProgressBarArguments> HandleProgressBar;
    public event EventHandler OnCut;
    public static event EventHandler OnAnyCutPlaySound;

    public new static void ResetEvents()
    {
        OnAnyCutPlaySound = null;
    }
    
    
    public override void Interact(IKitchenObjectParent player)
    {
        if (player.HasKitchenObject())
        {
            if (!HasKitchenObject())
            { 
                if (HasRecipeInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenParent(this);
                    SetCuttingProgressToZeroServerRpc();
                }
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
            }
            
        }
        else
        {
            if (HasKitchenObject())
            {
                if (HasRecipeInput(GetKitchenObject().GetKitchenObjectSO()))
                {
                    SetCuttingProgressToZeroServerRpc();
                }
                kitchenObject.SetKitchenParent(player);
            }
        }
    }

    public override void CutInteraction()
    {
        if (HasKitchenObject() && HasRecipeInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutInteractionServerRpc();
            TryGetKitchenObjectCutOutputServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutInteractionServerRpc()
    {
        if (HasKitchenObject() && HasRecipeInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutInteractionClientRpc();
        }
    }

    [ClientRpc]
    private void CutInteractionClientRpc()
    {
        KitchenRecipeSoTemplate recipeOutput = GetRecipeTemplateFromInput(GetKitchenObject().GetKitchenObjectSO());
        cuttingProgress++;
        cuttingProgress = Mathf.Clamp(cuttingProgress, 0,recipeOutput.maxCuttingProgress);
        OnCut?.Invoke(this,EventArgs.Empty);
        OnAnyCutPlaySound?.Invoke(this, EventArgs.Empty);
        HandleProgressBar?.Invoke(this,new IhasProgressBar.ProgressBarArguments
        { 
            progressBarFill = (float) cuttingProgress / recipeOutput.maxCuttingProgress
        });
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryGetKitchenObjectCutOutputServerRpc()
    {
        if (HasKitchenObject() && HasRecipeInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenRecipeSoTemplate recipeOutput = GetRecipeTemplateFromInput(GetKitchenObject().GetKitchenObjectSO());
            if (recipeOutput != null && cuttingProgress >= recipeOutput.maxCuttingProgress)
            {

                GetKitchenObject().DestroySelf();
                KitchenObject.CreateKitchenObject(recipeOutput.GetOutput(), this);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetCuttingProgressToZeroServerRpc()
    {
        SetCuttingProgressToZeroClientRpc();
    }
    
    [ClientRpc]
    private void SetCuttingProgressToZeroClientRpc()
    {
        cuttingProgress = 0;
        HandleProgressBar?.Invoke(this,new IhasProgressBar.ProgressBarArguments
        {
            progressBarFill = 0f
        });
    }

    private KitchenObjectSO GetRecipeOutputFromInput(KitchenObjectSO recipeInput)
    {
        KitchenObjectSO kitchenObjectSo = GetRecipeTemplateFromInput(recipeInput).GetOutput();
        if (kitchenObjectSo != null)
        {
            return kitchenObjectSo;
        }

        return null;
    }

    private bool HasRecipeInput(KitchenObjectSO recipeInput)
    {
        return GetRecipeTemplateFromInput(recipeInput) != null;
    }
    
    private KitchenRecipeSoTemplate GetRecipeTemplateFromInput(KitchenObjectSO recipeInput)
    {
        foreach (var r in Recipes)
        {
            if (r.GetInput() == recipeInput)
            {
                return r;
            }
        }
        
        return null;
    }
    
}
