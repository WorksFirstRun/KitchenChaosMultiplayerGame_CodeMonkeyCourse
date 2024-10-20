using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorAndVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private MeshRenderer headMeshRender;

    private Material _material;

    private void Awake()
    {
        _material = new Material(headMeshRender.material);
        headMeshRender.material = _material;
        bodyMeshRenderer.material = _material;
    }
    

    public void SetPlayerColor(Color color)
    {
        _material.color = color;
    }
}
