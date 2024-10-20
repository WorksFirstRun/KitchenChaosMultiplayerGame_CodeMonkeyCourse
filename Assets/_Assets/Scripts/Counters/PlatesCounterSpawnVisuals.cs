using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterSpawnVisuals : MonoBehaviour
{
    [SerializeField] private Transform plateVisual;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> visualsList;

    private void Start()
    {
        visualsList = new List<GameObject>();
        platesCounter.onPlatesVisualSpawn += PlatesCounterOnPlatesVisualSpawn;
        platesCounter.onPlatesVisualRemove += PlatesCounterOnPlatesVisualRemove;
    }
    

    private void PlatesCounterOnPlatesVisualSpawn(object sender, EventArgs e)
    {
        Transform plate = Instantiate(plateVisual, counterTopPoint);

        float offset = 0.1f;
        plate.transform.localPosition = new Vector3(0, offset * visualsList.Count, 0);
        visualsList.Add(plate.gameObject);
        
    }
    
    private void PlatesCounterOnPlatesVisualRemove(object sender, EventArgs e)
    {
        GameObject TopPlate = visualsList[visualsList.Count - 1];
        visualsList.Remove(TopPlate);
        Destroy(TopPlate);
    }
}
