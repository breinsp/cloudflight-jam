using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public abstract void BuildingPlaced();

    public void SetVisible(bool visible)
    {
        if (meshRenderers == null) return;
        meshRenderers.ForEach(mr => mr.enabled = visible);
    }
}
