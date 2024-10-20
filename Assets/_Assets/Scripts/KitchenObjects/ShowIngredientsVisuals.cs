using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowIngredientsVisuals : MonoBehaviour
{
   [Serializable]
   public struct GameVisual_KitchenObjectTemplate
   {
      public GameObject visual;
      public KitchenObjectSO ingredient;
   }
   [SerializeField] private PlatesObject platesObject;
   [SerializeField] private List<GameVisual_KitchenObjectTemplate> visualsList;
   
   
   private void Start()
   {
      platesObject.onVisualsShow += PlatesObjectOnVisualsShow;
   }

   private void PlatesObjectOnVisualsShow(object sender, PlatesObject.VisualsShowArguments e)
   {
      foreach (GameVisual_KitchenObjectTemplate GK in visualsList)
      {
         if (e.KitchenObjectSo == GK.ingredient)
         {
            GK.visual.SetActive(true);
         }
      }
   }
   
   
}
