using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StoveRecipeSoTemplate : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] public float cookingTimer;
    
    public KitchenObjectSO GetInput()
    {
        return input;
    }

    public KitchenObjectSO GetOutput()
    {
        return output;
    }
}
