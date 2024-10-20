using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{
   [SerializeField] private FinalRecipesList finalRecipesList;
   private List<FinalRecipeSO> recipes;
   private float recipeOrderMaxTime = 5f;
   private float reciperOrderTimer;
   private int recipesMaxAmount = 4;
   public event EventHandler onRecipeVisualShow;
   public event EventHandler onPlaySuccessSound;
   public event EventHandler onPlayFailSound;
   public int successfulDelivers;
   
   public static DeliveryManager Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
      recipes = new List<FinalRecipeSO>();
   }

   private void Update()
   {
      if (!IsServer)
      {
         return;
      }
      
      reciperOrderTimer += Time.deltaTime;
      if (reciperOrderTimer >= recipeOrderMaxTime)
      {
         reciperOrderTimer = 0;
         if (recipesMaxAmount > recipes.Count && GameManager.Instance.IsGameRunning())
         {
            int randomIndex = Random.Range(0, finalRecipesList.finalRecipesList.Count);
            SpawnRecipeClientRpc(randomIndex);
         }
      }
   }

   [ClientRpc]
   private void SpawnRecipeClientRpc(int randomIndex)
   {
      FinalRecipeSO randomRecipe = finalRecipesList.finalRecipesList[randomIndex];
      recipes.Add(randomRecipe);
      onRecipeVisualShow?.Invoke(this , EventArgs.Empty);
   }
   
   public void Deliver_a_Recipe(List<KitchenObjectSO> recipe){
      for (int i = 0; i < recipes.Count; i++)
      {
         if (recipe.Count == recipes[i].finalRecipesList.Count)
         {
            bool recipesmatch = false;
            foreach (KitchenObjectSO kt in recipes[i].finalRecipesList)
            {
               bool Found = false;
               foreach (KitchenObjectSO recipeKT in recipe)
               {
                  if (kt == recipeKT)
                  {
                     Found = true;
                     break;
                  }
               }

               if (Found)
               {
                  recipesmatch = true;
               }
               
            }

            if (recipesmatch)
            {
               DeliverCorrectRecipeServerRpc(i);
               return;
            }
            
         }
      }

      DeliverNotCorrectRecipeServerRpc();
   }

   [ServerRpc(RequireOwnership = false)]
   private void DeliverNotCorrectRecipeServerRpc()
   {
      DeliverNotCorrectRecipeClientRpc();
   }

   [ClientRpc]
   private void DeliverNotCorrectRecipeClientRpc()
   {
      onPlayFailSound?.Invoke(this, EventArgs.Empty);
      GameManager.Instance.ChangeTimer(5f);
   }
   
   
   [ServerRpc(RequireOwnership = false)]
   private void DeliverCorrectRecipeServerRpc(int index)
   {
      DeliverCorrectRecipeClientRpc(index);
   }

   [ClientRpc]
   private void DeliverCorrectRecipeClientRpc(int index)
   {
      recipes.RemoveAt(index);
      onRecipeVisualShow?.Invoke(this , EventArgs.Empty);
      onPlaySuccessSound?.Invoke(this,EventArgs.Empty);
      successfulDelivers++;
      GameManager.Instance.ChangeTimer(-5f);
   }
   
   public List<FinalRecipeSO> GetWaitingRecipes()
   {
      return recipes;
   }
   

}
