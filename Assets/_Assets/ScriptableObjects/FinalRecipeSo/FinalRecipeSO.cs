using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FinalRecipeSO : ScriptableObject
{
    [SerializeField] public List<KitchenObjectSO> finalRecipesList;
    [SerializeField] public string recipeName;
}
