using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers;
    private Dictionary<MeshRenderer, Material[]> originalMaterials;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        originalMaterials = meshRenderers.ToDictionary(m1 => m1, m2 => m2.materials);
    }

    public abstract void BuildingPlaced();

    public void SetVisible(bool visible)
    {
        if (meshRenderers == null || originalMaterials == null) return;
        if (visible)
        {
            meshRenderers.ForEach(mr => mr.materials = originalMaterials[mr]);
        }
        else
        {
            meshRenderers.ForEach(mr => mr.materials = new Material[] { BuildSystem.instance.invalidMaterial });
        }
    }
}
