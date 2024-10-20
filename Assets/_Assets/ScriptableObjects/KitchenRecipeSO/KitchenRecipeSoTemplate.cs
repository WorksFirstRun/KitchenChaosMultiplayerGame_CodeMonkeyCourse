using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenRecipeSoTemplate : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    public int maxCuttingProgress;

    public KitchenObjectSO GetInput()
    {
        return input;
    }

    public KitchenObjectSO GetOutput()
    {
        return output;
    }
}
