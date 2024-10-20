using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounterScript , IhasProgressBar
{
    [SerializeField] private StoveRecipeSoTemplate[] recipes;
    private StoveRecipeSoTemplate recipe;
    private NetworkVariable<float> currentCookingTimer = new NetworkVariable<float>(0);
    private NetworkVariable<float> maxCookingTimer = new NetworkVariable<float>(1);
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);

    public event EventHandler<StoveState> onAnimationAction;
    public event EventHandler<IhasProgressBar.ProgressBarArguments> HandleProgressBar;

    public class StoveState : EventArgs
    {
        public State state;
    }
    
    public enum State
    { 
        Idle,
        Frying,
        Fried,
        Burned
    }
    

    public override void OnNetworkSpawn()
    {
        currentCookingTimer.OnValueChanged += NetworkVariable_OnValueChanged_Timer;
        state.OnValueChanged += NetworkVariable_OnValueChanged_State;
    }

    private void NetworkVariable_OnValueChanged_State(State previousvalue, State newvalue)
    {
        InvokeAnimationStateClientRpc(state.Value);
    }

    private void NetworkVariable_OnValueChanged_Timer(float previousvalue, float newvalue)
    {
        HandleProgressBar?.Invoke(this, new IhasProgressBar.ProgressBarArguments()
        {
            progressBarFill = currentCookingTimer.Value / maxCookingTimer.Value
        });
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        
        if (HasKitchenObject() && recipe != null)
        {
            switch (state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    currentCookingTimer.Value += Time.deltaTime;
                    if (currentCookingTimer.Value >= recipe.cookingTimer)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.CreateKitchenObject(recipe.GetOutput(), this);

                        recipe = GetRecipeTemplateFromInput(GetKitchenObject().GetKitchenObjectSO());
                        if (recipe == null) // handle the error where you put fried meat again and the animation stay active
                        {
                            state.Value = State.Burned;
                        }
                        else
                        {
                            state.Value = State.Fried;
                        }
                        maxCookingTimer.Value = recipe == null ? 1 : recipe.cookingTimer;
                        currentCookingTimer.Value = 0;
                    }
                    break;
                case State.Fried:
                    currentCookingTimer.Value += Time.deltaTime;
                    if (currentCookingTimer.Value >= recipe.cookingTimer)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.CreateKitchenObject(recipe.GetOutput(), this);
            
                        recipe = GetRecipeTemplateFromInput(GetKitchenObject().GetKitchenObjectSO());
                        
                        state.Value = State.Burned;
                        currentCookingTimer.Value = 0;
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
        
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
                    int recipeIndex = KitchenGameMultiPlayer.Instance.GetKitchenObjectIndex(kitchenObject.GetKitchenObjectSO());
                    ChangeStoveStateServerRpc(recipeIndex);
                    SetMaxRecipeCookingTimerServerRpc();
                }
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlatesObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        ResetTimerServerRpc();
                        kitchenObject.DestroySelf();
                    }
                }
            }
        }
        else
        {
            if (HasKitchenObject())
            {
                kitchenObject.SetKitchenParent(player);
                ResetTimerServerRpc();
            }
        }
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
    
    private StoveRecipeSoTemplate GetRecipeTemplateFromInput(KitchenObjectSO recipeInput)
    {
        foreach (var r in recipes)
        {
            if (r.GetInput() == recipeInput)
            {
                return r;
            }
        }

        return null;
    }

    private void InvokeAnimationAction(State state)
    {
        onAnimationAction?.Invoke(this, new StoveState()
        {
            state = state
        });
        
    }


    [ServerRpc (RequireOwnership = false)]
    private void ChangeStoveStateServerRpc(int recipeIndex)
    {
        state.Value = State.Frying;
        currentCookingTimer.Value = 0;
        ChangeStoveStateClientRpc(recipeIndex);
    }

    [ClientRpc]
    private void ChangeStoveStateClientRpc(int recipeIndex)
    {
        KitchenObjectSO recipeInput = KitchenGameMultiPlayer.Instance.GetKitchenObjectSoFromIndex(recipeIndex);
        recipe = GetRecipeTemplateFromInput(recipeInput);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetMaxRecipeCookingTimerServerRpc()
    {
        maxCookingTimer.Value = recipe == null ? 1 : recipe.cookingTimer;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ResetTimerServerRpc()
    {
        currentCookingTimer.Value = 0;
        state.Value = State.Idle;
    }

    [ClientRpc]
    private void InvokeAnimationStateClientRpc(State state)
    {
        InvokeAnimationAction(state);
    }

    
}
