using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FinalRecipesList : ScriptableObject
{
    [SerializeField] public List<FinalRecipeSO> finalRecipesList;
}
