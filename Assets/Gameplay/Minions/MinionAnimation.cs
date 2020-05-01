using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAnimation : MonoBehaviour
{
    public Material defaultMaterial;
    public Material sadMaterial;
    public Material angryMaterial;

    public SkinnedMeshRenderer meshRenderer;

    public void SetState(MinionState state)
    {
        switch (state)
        {
            case MinionState.normal: meshRenderer.sharedMaterial = defaultMaterial; break;
            case MinionState.fighting: meshRenderer.sharedMaterial = angryMaterial; break;
            case MinionState.sacrificing: meshRenderer.sharedMaterial = sadMaterial; break;
        }
    }
}

public enum MinionState
{
    normal,
    sacrificing,
    fighting
}
