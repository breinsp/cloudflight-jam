using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAnimation : MonoBehaviour
{
    public Material defaultMaterial;
    public Material sadMaterial;
    public Material angryMaterial;

    public SkinnedMeshRenderer meshRenderer;
    public Animator animator;

    public void SetState(MinionState state)
    {
        switch (state)
        {
            case MinionState.normal: meshRenderer.sharedMaterial = defaultMaterial; animator.Play("Running"); break;
            case MinionState.fighting: meshRenderer.sharedMaterial = angryMaterial; animator.Play("Fighting"); break;
            case MinionState.sacrificing: meshRenderer.sharedMaterial = sadMaterial; animator.Play("Sprinting"); break;
            case MinionState.praising: meshRenderer.sharedMaterial = sadMaterial; animator.SetTrigger("praise"); break;
            case MinionState.wait: meshRenderer.sharedMaterial = defaultMaterial; animator.SetTrigger("wait"); break;
        }
    }
}

public enum MinionState
{
    normal,
    sacrificing,
    fighting,
    praising,
    wait
}
