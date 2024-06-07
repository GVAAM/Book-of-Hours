using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    protected Material originalMat;
    [SerializeField] protected Material highlightMaterial;
    [SerializeField] protected Material clickedMaterial;
    protected MeshRenderer meshRenderer;
    private bool isOn = false; // for modes 

    private void Awake()
    {
        isOn = false;
        meshRenderer = GetComponent<MeshRenderer>();
        originalMat = meshRenderer.material;
    }

    public void setHighlightMat(Material mat)
    {
        highlightMaterial = mat;
    }

    public void setOriginalMat(Material mat)
    {
        originalMat = mat;
    }

    public void setClickedMat(Material mat)
    {
        clickedMaterial = mat;
    }

    public void TurnOnHighlight(Color highlightColor)
    {
        if (!isOn)
        {
            meshRenderer.material = highlightMaterial;
            meshRenderer.material.SetColor("_BaseColor", highlightColor);
        }
    }
    
    public void TurnOnHighlight()
    {
        meshRenderer.material = highlightMaterial;
    }

    public void TurnOffHighlight()
    {
        if (!isOn)
        {
            meshRenderer.material = originalMat;
        }
        else
        {
            meshRenderer.material = clickedMaterial;
        }
    }

    public void TurnOnClickedMaterial()
    {
        if (!isOn)
        { 
            meshRenderer.material = clickedMaterial;
        }
    }
    
    public void TurnOffClickedMaterial()
    {
        if (!isOn)
        { 
            meshRenderer.material = highlightMaterial;
        }
    }

    public void ToggleClick()
    {
        if (isOn) // turn off 
        {
            meshRenderer.material = highlightMaterial;
            isOn = false;
        }
        else // turn on 
        {
            meshRenderer.material = clickedMaterial;
            isOn = true;
        }
    }

    public void ResetMode()
    {
        isOn = false;
        meshRenderer.material = originalMat;
    }
}
